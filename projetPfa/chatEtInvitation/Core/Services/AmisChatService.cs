using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using System;
using System.Threading.Tasks;

namespace chatEtInvitation.Core.Services
{
    public class AmisChatService :IchatAmisService
    {
        private readonly IChatAmisMessageRepository _chatRepository;
        private readonly IUserService _userService;
        private readonly IBlockService _blockService;


        public AmisChatService(
            IChatAmisMessageRepository chatRepository, IUserService userService, IBlockService blockService)
        {
            _chatRepository = chatRepository;
            _userService = userService;
            _blockService = blockService;
        }

        public async Task<List<AmisChatReturnedDTO>> GetAmisChatInfoAsync(int userId)
        {
            var chats = await _chatRepository.GetUserChatsAsync(userId);
            var result = new List<AmisChatReturnedDTO>();

            foreach (var chat in chats)
            {
                int amiId = chat.Member1 == userId ? chat.Member2 : chat.Member1;
                var ami = await _userService.FetchUserAsync(amiId, chat.AdminId);

                var lastMessage = await _chatRepository.GetLastChatMessageAsync(chat.Id);
                var unreadCount = await _chatRepository.GetUnreadMessagesCountAsync(chat.Id, userId);

                // Filtrage correct avec IsTeam = false
                var statut = lastMessage?.Statuts?
                    .FirstOrDefault(ms => ms.UtilisateurId == userId && ms.IsTeam == false)?
                    .Statut?.libelle;

                result.Add(new AmisChatReturnedDTO
                {
                    Id = chat.Id,
                    AmiName = $"{ami.Prenom} {ami.Nom}",
                    LastMessage = lastMessage?.Contenue,
                    Date = lastMessage?.when ?? DateTime.MinValue,
                    UnreadCount = unreadCount > 0 ? unreadCount : null,
                    Statut = statut, // Maintenant correctement peuplé
                    Avatar = ami.ImageUrl,
                    idMember= amiId
                });
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }

        public async Task<AmisMessageDTO> SendAmisMessageAsync(SendAmisMessageDTO messageDto)
        {
            var amichat = await _chatRepository.GetChatAmisByIdAsync(messageDto.ChatAmisId);
            if (amichat == null)
            {
                throw new ArgumentException("Chat ami introuvable");
            }

            int recepteurId = messageDto.EmetteurId == amichat.Member1 ? amichat.Member2 : amichat.Member1;

            bool emetteurABloqueRecepteur = await _blockService.IsUserBlockedAsync(recepteurId, messageDto.EmetteurId);

            bool recepteurABloqueEmetteur = await _blockService.IsUserBlockedAsync(messageDto.EmetteurId, recepteurId);

            if (emetteurABloqueRecepteur || recepteurABloqueEmetteur)
            {
                throw new InvalidOperationException("Message non envoyé : relation bloquée");
            }

            var message = new ChatAmisMessage
            {
                ChatAmisId = messageDto.ChatAmisId,
                Emetteur = messageDto.EmetteurId,
                Contenue = messageDto.Contenu,
                when = DateTime.Now
            };

            var createdMessage = await _chatRepository.CreateAmisMessageAsync(message);

            var messageStatutEmetteur = new MessageStatut
            {
                MessageId = createdMessage.Id,
                StatutId = 2, // Statut "Sent"
                UtilisateurId = messageDto.EmetteurId,
                IsTeam = false
            };
            await _chatRepository.AddMessageStatutAsync(messageStatutEmetteur);

            var messageStatutRecepteur = new MessageStatut
            {
                MessageId = createdMessage.Id,
                StatutId = 1, 
                UtilisateurId = recepteurId,
                IsTeam = false
            };
            await _chatRepository.AddMessageStatutAsync(messageStatutRecepteur);

            var emetteur = await _userService.FetchUserAsync(messageDto.EmetteurId, messageDto.AdminId);

            return new AmisMessageDTO
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
                Statut = "Sent",
                RecepteurId=recepteurId,
                chatAmisId= amichat.Id,
                
            };
        }
        public async Task<PaginatedResponse<AmisMessageDTO>> GetAmisConversationAsync(int chatAmisId, int adminId , int page = 1,
        int pageSize = 20)
        {
            var messages = await _chatRepository.GetConversationAsync(chatAmisId,page,pageSize);
            var result = new List<AmisMessageDTO>();

            foreach (var message in messages.Items)
            {
                var emetteur = await _userService.FetchUserAsync(message.Emetteur, adminId);

                // Prendre le statut pour l'admin ou le membre (selon votre logique)
                var statut = message.Statuts.FirstOrDefault()?.Statut?.libelle;

                result.Add(new AmisMessageDTO
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

            return new PaginatedResponse<AmisMessageDTO>
            {
                Items = result,
                TotalCount = messages.TotalCount,
                Page = messages.Page,
                PageSize = messages.PageSize
            };
        }

        public async Task<bool> AmisChatCheck(int idMember1, int idMember2, int AdminId)
        {
            return await _chatRepository.AmisChatCheck(idMember1, idMember2, AdminId) != null;
        }





    }
}
