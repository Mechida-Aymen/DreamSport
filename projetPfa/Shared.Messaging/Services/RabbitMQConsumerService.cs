using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Messaging.Services
{
    public abstract class RabbitMQConsumerService<TService> : BackgroundService where TService : class
    {
        private readonly string _hostname = "rabbitmq";  // Direct configuration here
        private readonly string _username = "user";      // Direct configuration here
        private readonly string _password = "password";
        private readonly string _queueName;
        private readonly TService _service;
        private IConnection _connection;
        private IModel _channel;

        protected RabbitMQConsumerService(string queueName, TService service)
        {
            _queueName = queueName;
            _service = service;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Message reçu: {message}");

                try
                {
                    var receivedData = JsonSerializer.Deserialize<dynamic>(message);
                    await ProcessMessageAsync(receivedData, _service);

                    _channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine("Message traité et supprimé.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du traitement du message : {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        protected abstract Task ProcessMessageAsync(dynamic message, TService service);

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
