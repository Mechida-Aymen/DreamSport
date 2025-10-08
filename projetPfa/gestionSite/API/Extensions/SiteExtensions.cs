

using gestionSite.Core.Interfaces.SiteInterfaces;
using gestionSite.Core.Services;
using gestionSite.Infrastructure.Data.Repositories;

namespace gestionSite.API.Extensions
{
    public static class SiteExtensions
    {

        public static void AddSiteDependencies(this IServiceCollection services)
        {
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<ISiteRepository, SiteRepository>();
        

        }

    }
}
