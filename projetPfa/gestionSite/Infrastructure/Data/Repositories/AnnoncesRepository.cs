using gestionSite.Core.Interfaces.AnnoncesInterfaces;
using gestionSite.Core.Models;

namespace gestionSite.Infrastructure.Data.Repositories
{
    public class AnnoncesRepository: IAnnoncesRepository
    {

        private readonly AppDbContext _context; // Replace with your DbContext name

        public AnnoncesRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Annonces>> GetAllComplexAnnoncesAsync(int idAdmin)
        {
            return await _context.Annonces
                                 .Where(f => f.IdAdmin == idAdmin)
                                 .ToListAsync();
        }

        public async Task<Annonces?> AddAnnoncesAsync(Annonces _annonces)
        {
            try
            {
                var result = await _context.Annonces.AddAsync(_annonces);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                // Handle exceptions as needed (e.g., log the error)
                return null;
            }
        }

        public async Task<Annonces?> DeleteAnnoncesAsync(int id)
        {
            var annonces = await _context.Annonces.FindAsync(id);
            if (annonces == null)
            {
                return null;
            }

            _context.Annonces.Remove(annonces);
            await _context.SaveChangesAsync();
            return annonces;
        }
    }
}
