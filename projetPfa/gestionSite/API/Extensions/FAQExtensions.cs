using gestionSite.Core.Interfaces.FAQ;
using gestionSite.Core.Interfaces.FAQInterfaces;
using gestionSite.Core.Services;
using gestionSite.Infrastructure.Data.Repositories;

namespace gestionSite.API.Extensions
{
    public static class FAQExtensions
    {
        public static void AddFaqDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFAQService, FAQService>();
            services.AddScoped<IFAQRepository, FAQRepository>();

        }
    }
}
