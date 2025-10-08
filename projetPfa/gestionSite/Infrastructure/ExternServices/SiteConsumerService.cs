using Microsoft.Extensions.DependencyInjection;
using gestionSite.Core.Interfaces.SiteInterfaces;
using Shared.Messaging.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using gestionSite.API.DTOs;

public class SiteConsumerService : RabbitMQConsumerService<ISiteService>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SiteConsumerService(IServiceScopeFactory serviceScopeFactory)
        : base("Creation de site", null) // Service null, on le récupère via scope
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ProcessMessageAsync(dynamic message, ISiteService siteService)
    {
        try
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jsonMessage = JsonSerializer.Deserialize<AdminMessageDTO>(message.ToString(), jsonOptions);

            if (jsonMessage == null)
            {
                Console.WriteLine("Erreur de désérialisation du message.");
                return;
            }

            Console.WriteLine($"Création du site pour l'admin: {jsonMessage.Name}, ID: {jsonMessage.Id}");

            using var scope = _serviceScopeFactory.CreateScope();
            var scopedSiteService = scope.ServiceProvider.GetRequiredService<ISiteService>();

            var result = await scopedSiteService.AddSiteAsync(jsonMessage.Id);

            if (result != null)
            {
                Console.WriteLine($" Site créé avec succès pour l'admin: {jsonMessage.Name}, ID: {jsonMessage.Id}");
            }
            else
            {
                Console.WriteLine($" Erreur lors de la création du site pour l'admin: {jsonMessage.Name}, ID: {jsonMessage.Id}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Erreur lors du traitement du message : {ex.Message}");
        }
    }
}
