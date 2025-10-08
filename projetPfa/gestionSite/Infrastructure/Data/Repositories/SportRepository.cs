using gestionSite.Core.Interfaces.SportInterfaces;
using gestionSite.Core.Models;

namespace gestionSite.Infrastructure.Data.Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly AppDbContext _context;
        public SportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sport_Categorie>> GetSportsAsync()
        {
            return await _context.Sports.ToListAsync();

        }

    }
}
