using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;
using System;

namespace chatEtInvitation.API.Mappers
{
    public class ChatMapper
    {
       

        public ChatDto ToDto(AmisChat chat)
        {
            return new ChatDto
            {
                Id = chat.Id,
                Member1Id = chat.Member1,
                Member2Id = chat.Member2,
                AdminId = chat.AdminId,
            };
        }
    }

}
