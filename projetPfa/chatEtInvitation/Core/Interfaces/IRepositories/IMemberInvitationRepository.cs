using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface IMemberInvitationRepository
    {
        Task<MemberInvitation> GetMemberInvitationAsync(int emetteur, int recepteur);
        Task AddInvitationAsync(MemberInvitation invitation);
        Task<bool> RefuserInvitation(MemberInvitation inv  );
        Task<MemberInvitation> GetInvitationByIdAsync(int id);
        Task<MemberInvitation> AccepterInvitationAsync(int invitationId);
        Task<List<MemberInvitation>> GetUserInvitationsAsync(int userId);
        Task<int> GetUserInvitationsCountAsync(int userId, int adminId);




    }
}
