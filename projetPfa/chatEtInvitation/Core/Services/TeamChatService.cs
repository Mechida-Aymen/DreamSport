using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using chatEtInvitation.Infrastructure.Data.Repositories;
using chatEtInvitation.Infrastructure.ExternServices;
using chatEtInvitation.Infrastructure.ExternServices.ExternDTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Core.Services
{
    public class TeamChatService : IchatTeamService
    {
        private readonly ITeamMessageRepository _chatRepository;
        private readonly ITeamService _TeamService;
        private readonly IUserService _userService;
        private readonly ITeamChatRepository _teamChatRepository;

        public TeamChatService(
     ITeamMessageRepository chatRepository,
     ITeamService TeamService,
     IUserService userService,
     ITeamChatRepository teamChatRepository)
        {
            _chatRepository = chatRepository;
            _TeamService = TeamService;
            _userService = userService;
            _teamChatRepository = teamChatRepository;

        }

        public async Task<TeamChatReturnedDTO> GetTeamChatByIdAsync(int idEquipe, int idMember)
        {
            var teamChat = await _chatRepository.ExisteChatTeamAsync(idEquipe);
            if (teamChat == null) return null;

            teamDTO team = await _TeamService.FetchTeamAsync(teamChat.TeamId, teamChat.AdminId);
            var lastMessage = await _chatRepository.GetLastTeamMessageAsync(teamChat.Id);
            var statut = lastMessage != null
                ? await _chatRepository.GetUserMessageStatutAsync(lastMessage.Id, idMember)
                : null;
            var unreadCount = await _chatRepository.GetUnreadMessagesCountAsync(teamChat.Id, idMember);

            return new TeamChatReturnedDTO
            {
                Id = teamChat.Id,
                EquipeName = team.Name,
                lasteMessage = lastMessage?.Contenue,
                date = lastMessage?.when ?? DateTime.MinValue,
                nbrMessage = unreadCount,
                st = statut,
                avatar = team.Avatar
            };
        }

        public async Task<PaginatedResponse<TeamMessageDTO>> GetFullTeamConversationAsync(int teamId, int adminId, int page = 1, int pageSize = 20)
        {
            var teamChat = await _chatRepository.ExisteChatTeamAsync(teamId);
            if (teamChat == null) return null;

            var paginatedMessages = await _chatRepository.GetTeamConversationWithStatutsAsync(teamChat.Id, page, pageSize);
            var result = new List<TeamMessageDTO>();

            foreach (var message in paginatedMessages.Items)
            {
                var emetteur = await _userService.FetchUserAsync(message.Emetteur, adminId);
                var statut = message.Statuts.FirstOrDefault()?.Statut?.libelle;

                result.Add(new TeamMessageDTO
                {
                    Id = message.Id,
                    Contenu = message.Contenue,
                    DateEnvoi = message.when,
                    Emetteur = new UserInfoDTO
                    {
                        Id = emetteur.Id,
                        NomComplet = $"{emetteur.Prenom} {emetteur.Nom}",
                        Avatar = emetteur.ImageUrl
                    },
                    Statut = statut
                });
            }

            return new PaginatedResponse<TeamMessageDTO>
            {
                Items = result,
                TotalCount = paginatedMessages.TotalCount,
                Page = paginatedMessages.Page,
                PageSize = paginatedMessages.PageSize
            };
        }
        public async Task MarkMessagesAsSeenAsync(List<int> messageIds, int userId)
        {
            await _chatRepository.UpdateMessagesStatusAsync(messageIds, userId, 2);
        }

        public async Task MarkAllMessagesAsSeenAsync(int teamChatId, int userId)
        {
            var messages = await _chatRepository.GetTeamConversationAsync(teamChatId);
            var messageIds = messages.Select(m => m.Id).ToList();

            await _chatRepository.UpdateMessagesStatusAsync(messageIds, userId, 2);

        }

        public async Task<TeamMessageDTO> SendTeamMessageAsync(SendTeamMessageDTO messageDto, int adminId)
        {
            // Vérifier si le chat existe
            var teamChat = await _chatRepository.ExisteChatTeamAsync(messageDto.TeamId);
            if (teamChat == null)
            {
                throw new ArgumentException("Chat d'équipe introuvable");
            }

            // Créer le message
            var message = new TeamChatMessage
            {
                Emetteur = messageDto.EmetteurId,
                Contenue = messageDto.Contenu,
                when = DateTime.Now,
                TeamChatId = teamChat.Id
            };

            

            var createdMessage = await _chatRepository.CreateTeamMessageAsync(message);

            // Récupérer tous les membres de l'équipe
            teamDTO team = await _TeamService.FetchTeamAsync(teamChat.TeamId, teamChat.AdminId);

            if (team.Membres == null || team.Membres.Count == 0)
            {
                throw new InvalidOperationException("Aucun membre trouvé dans l'équipe");
            }



            foreach (var membre in team.Membres)
            {
                var messageStatut = new MessageStatut
                {
                    MessageId = createdMessage.Id,
                    StatutId = membre.UserId == messageDto.EmetteurId ? 1 : 3,
                    UtilisateurId = membre.UserId,
                    IsTeam = true
                };

                await _chatRepository.AddMessageStatutAsync(messageStatut);
            }

            // Récupérer les infos de l'émetteur
            var emetteur = await _userService.FetchUserAsync(messageDto.EmetteurId, adminId);

            return new TeamMessageDTO
            {
                Id = createdMessage.Id,
                Contenu = createdMessage.Contenue,
                DateEnvoi = createdMessage.when,
                Emetteur = new UserInfoDTO
                {
                    Id = emetteur.Id,
                    NomComplet = $"{emetteur.Prenom} {emetteur.Nom}",
                    Avatar = emetteur.ImageUrl
                },
                TeamMemberIds = team.Membres,
                Statut = "Sent",
                chatTeamId= teamChat.Id,
                teamId= teamChat.TeamId
            };
        }
        public async Task CreateTeamChat(int teamId,int adminId)
        {
            TeamChat team=new TeamChat { AdminId = adminId , TeamId=teamId};

            _teamChatRepository.CreateChatAsync(team);

        }
    }
}
