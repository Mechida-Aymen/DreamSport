using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Services;
using chatEtInvitation.Infrastructure.Data.Repositories;
using chatEtInvitation.Infrastructure.ExternServices;

namespace chatEtInvitation.API.Extentions
{
    public static class ServicesExtenstions
    {
        public static void addServicesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMemberInvitationService, MemberInvitationService>();
            services.AddScoped<ITeamInvitationService, TeamInvitationService>();

            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();

        }
    }
}
