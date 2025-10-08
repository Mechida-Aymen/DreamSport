using gestionSite.Core.Models;
namespace gestionSite.Core.Interfaces.FAQ
{
    public interface IFAQService
    {
        Task<IEnumerable<gestionSite.Core.Models.FAQ>> GetFAQsByAdminAsync(int adminId);
        Task<gestionSite.Core.Models.FAQ?> AddFAQAsync(gestionSite.Core.Models.FAQ faq);
        Task<gestionSite.Core.Models.FAQ?> DeleteFAQAsync(int faqId);
        Task<gestionSite.Core.Models.FAQ?> UpdateFAQAsync(gestionSite.Core.Models.FAQ faq);
    }
}
