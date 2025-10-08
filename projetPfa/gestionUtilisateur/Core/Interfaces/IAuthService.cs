using gestionUtilisateur.API.DTOs;

namespace gestionUtilisateur.Core.Interfaces
{
    public interface IAuthService
    {
        Task UpdateTokenAsync(ReturnedLoginDto dto, int AdminId);
    }
}
