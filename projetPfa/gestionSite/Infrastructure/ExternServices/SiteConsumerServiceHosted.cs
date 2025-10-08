using gestionSite.Core.Interfaces.SiteInterfaces;
using Shared.Messaging.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class SiteConsumerServiceHosted : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SiteConsumerServiceHosted(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Créez un scope pour obtenir une instance de SiteConsumerService
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var siteConsumerService = scope.ServiceProvider.GetRequiredService<SiteConsumerService>();
            return siteConsumerService.StartAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Vous pouvez gérer l'arrêt du consommateur si nécessaire
        return Task.CompletedTask;
    }
}
