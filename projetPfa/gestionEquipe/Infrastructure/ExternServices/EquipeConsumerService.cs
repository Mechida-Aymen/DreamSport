using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using gestionEquipe.Infrastructure.ExternServices.ExternDTO;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

public class EquipeConsumerService : RabbitMQConsumerService<IMembersService>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EquipeConsumerService(IServiceScopeFactory serviceScopeFactory)
        : base("Add Member to team", null) 
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ProcessMessageAsync(dynamic message, IMembersService MemberService)
    {
        try
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jsonMessage = JsonSerializer.Deserialize<InvitationMessageDTO>(message.ToString(), jsonOptions);

            if (jsonMessage == null)
            {
                Console.WriteLine("Erreur de désérialisation du message.");
                return;
            }

            Console.WriteLine($"Ajouter l utilisateur: {jsonMessage.UserID}, a l equipe : {jsonMessage.EquipeId}");

            using var scope = _serviceScopeFactory.CreateScope();
            var scopedSiteService = scope.ServiceProvider.GetRequiredService<IMembersService>();

            Members membrer= new Members();
            membrer.UserId=jsonMessage.UserID;
            membrer.EquipeId = jsonMessage.EquipeId;

            var result = await scopedSiteService.AjouterMemberAsync(membrer);

            if (result != null)
            {
                Console.WriteLine($"Succes l utilisateur: {jsonMessage.UserID}, a l equipe : {jsonMessage.EquipeId}");
            }
            else
            {
                Console.WriteLine($"Erreur l utilisateur: {jsonMessage.UserID}, a l equipe : {jsonMessage.EquipeId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Erreur lors du traitement du message : {ex.Message}");
        }
    }
}
