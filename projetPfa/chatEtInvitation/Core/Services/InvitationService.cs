using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace chatEtInvitation.Core.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IMemberInvitationRepository _memberInvitationRepository;
        private readonly IAmisChatRepository _amisChatRepository;



        // Injection du repository dans le service
        public InvitationService(IMemberInvitationRepository memberInvitationRepository, IAmisChatRepository amisChatRepository)
        {
            _memberInvitationRepository = memberInvitationRepository;
            _amisChatRepository = amisChatRepository;
        }

        // Méthode pour refuser (supprimer) une invitation
        public async Task<bool> RefuserInvitation(int Id )
        {
            MemberInvitation inv = await _memberInvitationRepository.GetInvitationByIdAsync(Id);
            if (inv==null)
            {
                throw new KeyNotFoundException("Invitation not found");
            }
            return await _memberInvitationRepository.RefuserInvitation(inv);
        }

        //Get invitation by ID

        public async Task<Invitation> GetInvitationByIdAsync(int id)
        {
            var invitation = await _memberInvitationRepository.GetInvitationByIdAsync(id);
            return invitation;

        }




        // Méthode pour accepter une invitation
        public async Task<bool> AccepterInvitationAsync(int invitationId)
        {
            MemberInvitation invi = await _memberInvitationRepository.GetInvitationByIdAsync(invitationId);
            if (invi == null)
            {
                throw new KeyNotFoundException("Invitation not found");
            }
            await _memberInvitationRepository.RefuserInvitation(invi);
            AmisChat chat = new AmisChat { Member1 = invi.Emetteur, Member2 = invi.Recerpteur,AdminId = invi.AdminId};
            await _amisChatRepository.CreateChatAsync(chat);
            return true;
        }
    }

}
