using chatEtInvitation.Infrastructure.ExternServices.Extern_DTo;

namespace chatEtInvitation.Core.Interfaces.IExternServices
{
    public interface IUserService
    {
        Task<UserDTO> FetchUserAsync(int idUser, int adminId);
    }
}
