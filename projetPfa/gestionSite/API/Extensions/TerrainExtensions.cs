using gestionSite.Core.Interfaces.TerrainInterfaces;
using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;
using gestionSite.Core.Services;
using gestionSite.Infrastructure.Data.Repositories;
using gestionSite.Infrastructure.Repositories;

namespace gestionSite.API.Extensions
{
    public static class TerraiExtensions
    {
        public static void AddTerrainDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITerrainService, TerrainService>();
            services.AddScoped<ITerrainRepository, TerrainRepository>();
            services.AddScoped<ITerrainStatusService, TerrainStatusService>();
            services.AddScoped<ITerrainStatusRepository, TerrainStatusRepository>();

        }
    }
}
