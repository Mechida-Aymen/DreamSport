using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Services;
using gestionUtilisateur.Infrastructure.Data.Repositories;

namespace gestionUtilisateur.API.Extensions
{
    public static class AnnoncesExtensions
    {
        public static void addUserDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}




