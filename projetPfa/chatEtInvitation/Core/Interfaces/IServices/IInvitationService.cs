using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IInvitationService
    {
        Task<bool> RefuserInvitation(int id );
        Task<Invitation> GetInvitationByIdAsync(int id);
        Task<bool> AccepterInvitationAsync(int invitationId);


    }
}
