using gestionReservation.Core.Models;

namespace gestionReservation.Core.Interfaces
{
    public interface IStatusRepository
    {
        Task<Status> GetStatusByLibelle(string libelle);
    }
}
