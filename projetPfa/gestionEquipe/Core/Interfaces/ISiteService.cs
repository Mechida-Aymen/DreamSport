using gestionEquipe.Infrastructure.ExternServices.ExternDTO;

namespace gestionEquipe.Core.Interfaces
{
    public interface ISiteService
    {
        Task<List<SportCategorieDTO>> GetSportsAsync(int adminId);
    }
}
