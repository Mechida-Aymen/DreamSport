using gestionSite.Core.Interfaces.SportInterfaces;
using gestionSite.Core.Models;

namespace gestionSite.Core.Services
{
    public class SportService : ISportService
    {
        private readonly ISportRepository _repository;

        public SportService(ISportRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Sport_Categorie>> GetSportsAsync()
        {
            return await _repository.GetSportsAsync();
        }

    }
}
