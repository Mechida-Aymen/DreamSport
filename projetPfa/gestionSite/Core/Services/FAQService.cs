using gestionSite.Core.Interfaces.FAQ;
using gestionSite.Core.Interfaces.FAQInterfaces;
using gestionSite.Core.Models;

namespace gestionSite.Core.Services
{
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;

        public FAQService(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task<IEnumerable<FAQ>> GetFAQsByAdminAsync(int adminId)
        {
            // Business logic can be added here (e.g., validations)
            return await _faqRepository.GetAllComplexFAQsAsync(adminId);
        }

        public async Task<FAQ?> AddFAQAsync(FAQ faq)
        {
            // Add any necessary validation or preprocessing here
            if (await _faqRepository.ExistsAsync(faq.Question,faq.IdAdmin))
            {
                return null;
            }

            return await _faqRepository.AddFaqAsync(faq);
        }

        public async Task<FAQ?> DeleteFAQAsync(int faqId)
        {

            // Optionally check if the FAQ exists before deleting
            var existingFaq = await _faqRepository.DeleteFaqAsync(faqId);
            if (existingFaq == null)
            {
                // Log or handle "not found" scenario
                return null;
            }

            return existingFaq;
        }

        public async Task<FAQ?> UpdateFAQAsync(FAQ faq)
        {
            // Add any necessary validation or preprocessing here
            if (faq.Id <= 0)
            {
                // Example validation: ID must be valid
                return null;
            }

            return await _faqRepository.UpdateFaqAsync(faq);
        }
    }
}
