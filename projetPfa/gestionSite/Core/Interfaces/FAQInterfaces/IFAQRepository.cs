using gestionSite.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace gestionSite.Core.Interfaces.FAQInterfaces
{
    public interface IFAQRepository
    {
        Task<IEnumerable<gestionSite.Core.Models.FAQ>> GetAllComplexFAQsAsync(int idAdmin);
        Task<gestionSite.Core.Models.FAQ?> AddFaqAsync(gestionSite.Core.Models.FAQ _faq);
        Task<gestionSite.Core.Models.FAQ?> DeleteFaqAsync(int id);
        Task<gestionSite.Core.Models.FAQ?> UpdateFaqAsync(gestionSite.Core.Models.FAQ _faq);
        Task<bool> ExistsAsync(string question,int adminId);

    }
}
