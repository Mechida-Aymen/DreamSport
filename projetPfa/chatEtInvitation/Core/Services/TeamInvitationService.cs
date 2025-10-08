using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.API.DTOs;
using System.Net.Http;
using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Models;
using chatEtInvitation.Infrastructure.Data.Repositories;
using Shared.Messaging.Services;

namespace chatEtInvitation.Core.Services
{
    public class TeamInvitationService : ITeamInvitationService
    {
        private readonly ITeamInvitationRepository _TeamInvitationRepository;
        private readonly ITeamService _TeamService;

        public TeamInvitationService(ITeamInvitationRepository teamInvitationRepository, ITeamService teamService)
        {
            _TeamInvitationRepository = teamInvitationRepository;
            _TeamService = teamService;
        }

        public async Task AccepteInvitationAsync(int invId)
        {
            TeamInvitation invitation = await _TeamInvitationRepository.GetInvitationByIdAsync(invId);
            if(invitation == null)
            {
                throw new KeyNotFoundException("The invitation not found");
            }

            await _TeamInvitationRepository.DeleteInvitationAsync(invitation);

            var _producer = new RabbitMQProducerService("Add Member to team");
            // Utilisation de RabbitMQ Producer injecté
            _producer.Publish(new { UserID = invitation.Recerpteur , EquipeId = invitation.Emetteur });
        }

        public async Task<TeamInvitation> SendInvitationAsync(TeamInvitationDTO invitationDto)
        {
            // Vérifier si l'invitation existe déjà
            var existingInvitation = await _TeamInvitationRepository.GetExistingInvitationAsync(invitationDto.Emetteur, invitationDto.Recepteur);
            if (existingInvitation != null)
            {
                throw new KeyNotFoundException("The invitation not found");

            }

            // Vérifier si l'utilisateur est déjà dans l'équipe
            List<int> MembersIds = await _TeamService.FetchMembersAsync(invitationDto.Emetteur, invitationDto.AdminId);
            if (MembersIds == null )
            {
                throw new KeyNotFoundException("Team not found");
            }
            if (MembersIds.Contains(invitationDto.Recepteur))
            {
                throw new KeyNotFoundException("Already member");
            }

            // Ajouter l'invitation
            var invitation = TeamInvMapper.SendInvToModel(invitationDto);
            await _TeamInvitationRepository.AddInvitationAsync(invitation);

            return invitation;
        }

              //---------------------------------

        public async Task<List<MemberTeamInvitationDTOO>> GetUserTeamInvitationsAsync(int userId)
        {
            var invitations = await _TeamInvitationRepository.GetUserTeamInvitationsAsync(userId);

            return invitations.Select(inv => new MemberTeamInvitationDTOO
            {
                Id = inv.Id,
                Emetteur = inv.Emetteur,
                Recerpteur = inv.Recerpteur,
                AdminId = inv.AdminId,
            }).ToList();
        }

        // Méthode pour refuser (supprimer) une invitation
        public async Task RefuserInvitation(int Id)
        {
            TeamInvitation invitation = await _TeamInvitationRepository.GetInvitationByIdAsync(Id);
            if (invitation == null)
            {
                throw new KeyNotFoundException("The invitation not found");
            }

            await _TeamInvitationRepository.DeleteInvitationAsync(invitation);
            
            
        }


        // Méthode pour récupérer les invitations team et le nombre total
        public async Task<UserTeamInvitationsResponseDto> GetUserTeamInvitationsNbrAsync(int userId, int adminId)
        {
            // Récupérer les invitations
            var invitations = await _TeamInvitationRepository.GetUserTeamInvitationsAsync(userId);

            // Récupérer le nombre total d'invitations
            var totalData = await _TeamInvitationRepository.GetUserTeamInvitationsCountAsync(userId, adminId);

            // Mapper les entités vers les DTOs
            var invitationDtos = invitations.Select(inv => new MemberTeamInvitationDTOO
            {
                Id= inv.Id,
                Emetteur = inv.Emetteur,
                Recerpteur = inv.Recerpteur,
                AdminId = inv.AdminId,
            }).ToList();

            // Retourner la réponseMemberTeamInvitationDTOO
            return new UserTeamInvitationsResponseDto
            {
                Invitations = invitationDtos,
                TotalData = totalData
            };
        }
    }
}
