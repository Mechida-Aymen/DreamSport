using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Interfaces
{
    public interface IMembersService
    {
        Task<Members> KickMemberAsync(Members member);
        Task<Members> AjouterMemberAsync(Members member);
    }
}
