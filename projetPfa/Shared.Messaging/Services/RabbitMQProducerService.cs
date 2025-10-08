using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging.Services
{
    public class RabbitMQProducerService
    {
        private readonly string _hostname = "rabbitmq";
        private readonly string _username = "user";      // Direct configuration here
        private readonly string _password = "password";
        private readonly string _queueName;

        public RabbitMQProducerService(string queueName)
        {
            _queueName = queueName;
        }

        public void Publish(object message)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                string jsonMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

                Console.WriteLine($"[x] Message sent: {jsonMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while sending message: {ex.Message}");
            }
        }
    }
}
