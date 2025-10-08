using System.Text.Json;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Infrastructure.ExternServices.ExternDTOs;
using Shared.Messaging.Services;

namespace chatEtInvitation.Infrastructure.ExternServices
{
    public class TeamChatConsumerService : RabbitMQConsumerService<IchatTeamService>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TeamChatConsumerService(IServiceScopeFactory serviceScopeFactory) : base("Create chat equipe", null)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ProcessMessageAsync(dynamic message, IchatTeamService ChatTeamService)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonMessage = JsonSerializer.Deserialize<CreateTeamChatDTO>(message.ToString(), jsonOptions);

                if (jsonMessage == null)
                {
                    Console.WriteLine("Erreur de désérialisation du message.");
                    return;
                }

                Console.WriteLine($"Ajouter l utilisateur: {jsonMessage.teamId}, a l equipe : {jsonMessage.AdminId}");

                using var scope = _serviceScopeFactory.CreateScope();
                var scopedSiteService = scope.ServiceProvider.GetRequiredService<IchatTeamService>();


                var result = await scopedSiteService.CreateTeamChat(jsonMessage.teamId, jsonMessage.AdminId);

                if (result != null)
                {
                    Console.WriteLine($"Succes l utilisateur: {jsonMessage.teamId}, a l equipe : {jsonMessage.AdminId}");
                }
                else
                {
                    Console.WriteLine($"Erreur l utilisateur: {jsonMessage.teamId}, a l equipe : {jsonMessage.AdminId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erreur lors du traitement du message : {ex.Message}");
            }
        }

    }
}
