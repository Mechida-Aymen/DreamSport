using gestionReservation.Core.Interfaces;
using gestionReservation.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionReservation.Infrastructure.Data.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext _context;

        public StatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Status> GetStatusByLibelle(string libelle)
        {
            return await _context.Status.FirstOrDefaultAsync(s => s.Libelle == libelle);
        }

    }
}
