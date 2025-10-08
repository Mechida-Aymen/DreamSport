using gestionReservation.Infrastructure.ExternServices.Extern_DTo;

namespace gestionReservation.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> FetchUserAsync(int idUser , int adminId);
        Task<bool> ResetConteurResAnnulerAsync(int id, int adminId);
        Task<bool> CheckAndIncrementReservationAnnuleAsync(int userId, int adminId);
    }
}
