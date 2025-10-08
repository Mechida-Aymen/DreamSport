using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Infrastructure.Data.Repositories;

namespace chatEtInvitation.API.Extentions
{
    public static class RepositoriesExtenstions
    {
        public static void addRepositoriesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAmisChatRepository, AmisChatRepository>();

            services.AddScoped<IBloqueListRepository, BloqueListRepository>();

            services.AddScoped<IChatAmisMessageRepository, ChatAmisMessageRepository>();

            services.AddScoped<IMemberInvitationRepository, MemberInvitationRepository>();

            services.AddScoped<ITeamChatRepository, TeamChatRepository>();

            services.AddScoped<ITeamInvitationRepository, TeamInvitationRepository>();

            services.AddScoped<ITeamMessageRepository, TeamMessageRepository>();


        }
    }
}
