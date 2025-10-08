using Shared.Messaging.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class EquipeConsumerServiceHosted : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EquipeConsumerServiceHosted(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Créez un scope pour obtenir une instance de SiteConsumerService
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var EquipeConsumerService = scope.ServiceProvider.GetRequiredService<EquipeConsumerService>();
            return EquipeConsumerService.StartAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Vous pouvez gérer l'arrêt du consommateur si nécessaire
        return Task.CompletedTask;
    }
}
