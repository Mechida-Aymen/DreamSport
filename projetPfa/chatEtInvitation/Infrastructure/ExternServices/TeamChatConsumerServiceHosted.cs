namespace chatEtInvitation.Infrastructure.ExternServices
{
    public class TeamChatConsumerServiceHosted : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TeamChatConsumerServiceHosted(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Créez un scope pour obtenir une instance de SiteConsumerService
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var EquipeConsumerService = scope.ServiceProvider.GetRequiredService<TeamChatConsumerService>();
                return EquipeConsumerService.StartAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Vous pouvez gérer l'arrêt du consommateur si nécessaire
            return Task.CompletedTask;
        }
    }
}
