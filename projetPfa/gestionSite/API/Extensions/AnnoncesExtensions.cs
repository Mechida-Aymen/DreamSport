using gestionSite.Core.Interfaces.AnnoncesInterfaces;
using gestionSite.Core.Services;
using gestionSite.Infrastructure.Data.Repositories;

namespace gestionSite.API.Extensions
{
    public static class AnnoncesExtensions
    {
        public static void addAnnoncesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAnnoncesService, AnnoncesServices>();
            services.AddScoped<IAnnoncesRepository, AnnoncesRepository>();
        }
    }
}




