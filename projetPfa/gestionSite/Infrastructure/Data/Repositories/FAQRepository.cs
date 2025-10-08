using gestionSite.Core.Interfaces.FAQInterfaces;
using gestionSite.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace gestionSite.Infrastructure.Data.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        private readonly AppDbContext _context; // Replace with your DbContext name

        public FAQRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FAQ>> GetAllComplexFAQsAsync(int idAdmin)
        {
            return await _context.FAQs
                                 .Where(f => f.IdAdmin == idAdmin)
                                 .ToListAsync();
        }

        public async Task<FAQ?> AddFaqAsync(FAQ faq)
        {
            try
            {
                var result = await _context.FAQs.AddAsync(faq);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                // Handle exceptions as needed (e.g., log the error)
                return null;
            }
        }

        public async Task<FAQ?> DeleteFaqAsync(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null)
            {
                return null;
            }

            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();
            return faq;
        }

        public async Task<FAQ?> UpdateFaqAsync(FAQ faq)
        {
            var existingFaq = await _context.FAQs.FindAsync(faq.Id);
            if (existingFaq == null)
            {
                return null;
            }

            existingFaq.Question = faq.Question;
            existingFaq.Response = faq.Response;
            existingFaq.IdAdmin = faq.IdAdmin;

            _context.FAQs.Update(existingFaq);
            await _context.SaveChangesAsync();
            return existingFaq;
        }

        public async Task<bool> ExistsAsync(string question, int adminId)
        {
            return await _context.FAQs.AnyAsync(f => f.Question == question && f.IdAdmin == adminId);
        }

    }
}
