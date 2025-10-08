using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Interfaces
{
    public interface IMembersRepository
    {
        Task<Members> KickMemberAsync(Members member);
        Task<Members> KickkMemberAsync(Members member);

        Task<Members> AddMemberAsync(Members member);
        Task<Members> AddMemberSaveAsync(Members member);
        Task<bool> ExistInTeamAsync(Members member);
        Task<bool> ExistInTeamWithIdAsync(int IdMembre,int IdEquipe); 
        Task<int> CountTeamMembersAsync(int EquipeId);
        Task<List<Members>> GetTeamMembersAsync(int equipeId);
        Task<int> CountTeamsForMemberAsync(int MemberId);   
        
    }
}
