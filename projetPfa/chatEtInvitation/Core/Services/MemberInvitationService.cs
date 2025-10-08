using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Exceptions;
using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using chatEtInvitation.Infrastructure.Data.Repositories;
using chatEtInvitation.Infrastructure.ExternServices.Extern_DTo;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Core.Services
{
    public class MemberInvitationService : IMemberInvitationService
    {
        private readonly IMemberInvitationRepository _memberInvitationRepository;
        private readonly IUserService _userService;
        private readonly IBloqueListRepository _bloqueListRepository;


        public MemberInvitationService(IMemberInvitationRepository memberInvitationRepository, IBloqueListRepository bloqueListRepository, IUserService userService)
        {
            _memberInvitationRepository = memberInvitationRepository;
            _userService = userService;
            _bloqueListRepository = bloqueListRepository;

        }

        public async Task SendMemberInvitationAsync(MemberInvitation invitation)
        {
            MemberInvitation checkDirect = await _memberInvitationRepository.GetMemberInvitationAsync(invitation.Emetteur, invitation.Recerpteur);
            if (checkDirect != null)
            {
                throw new ConflictException("the invitation has been sended before");
            }
            MemberInvitation checkInvers = await _memberInvitationRepository.GetMemberInvitationAsync(invitation.Recerpteur, invitation.Emetteur);
            if (checkInvers != null)
            {
                throw new ConflictException("the invitation has been sended before");
            }

            BloqueList checkBloque = await _bloqueListRepository.IsBlockedAsync(invitation.Emetteur, invitation.Recerpteur);
            if (checkBloque != null)
            {
                throw new ForbiddenException("You are blocked by the that user");
            }
            checkBloque = await _bloqueListRepository.IsBlockedAsync(invitation.Recerpteur, invitation.Emetteur);
            if (checkBloque != null)
            {
                throw new ForbiddenException("You need to debloque the user first");
            }
            UserDTO Receiver = await _userService.FetchUserAsync(invitation.Recerpteur,invitation.AdminId);
            if (Receiver == null || Receiver.IdAdmin != invitation.AdminId)
            {
                throw new KeyNotFoundException("The Receiver not Found");
            }

            await _memberInvitationRepository.AddInvitationAsync(invitation);
        }

        public async Task<List<MemberInvitationDTO>> GetUserInvitationsAsync(int userId)
        {
            var invitations = await _memberInvitationRepository.GetUserInvitationsAsync(userId);

            return invitations.Select(inv => new MemberInvitationDTO
            {
                Id = inv.Id,
                Emetteur = inv.Emetteur,
                Recepteur = inv.Recerpteur,
            }).ToList();
        }


        // Méthode pour récupérer les invitations et le nombre total
        public async Task<UserInvitationsResponseDto> GetUserInvitationsNbrAsync(int userId, int adminId)
        {
            // Récupérer les invitations
            var invitations = await _memberInvitationRepository.GetUserInvitationsAsync(userId);

            // Récupérer le nombre total d'invitations
            var totalData = await _memberInvitationRepository.GetUserInvitationsCountAsync(userId, adminId);

            // Mapper les entités vers les DTOs
            var invitationDtos = invitations.Select(inv => new MemberInvitationDTOO
            {
                Id = inv.Id,
                Emetteur = inv.Emetteur,
                Recepteur = inv.Recerpteur,
                AdminId = inv.AdminId,
            }).ToList();

            // Retourner la réponse
            return new UserInvitationsResponseDto
            {
                Invitations = invitationDtos,
                TotalData = totalData
            };
        }
    }
}
