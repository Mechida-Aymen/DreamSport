using chatEtInvitation.Core.Models;

namespace chatEtInvitation.API.DTOs
{
    public class TeamChatReturnedDTO
    {

        public int Id { get; set; }
        public string EquipeName { get; set; }
        public string lasteMessage { get; set; }
        public DateTime date { get; set; }
        public int? nbrMessage { get; set; }
        public String st { get; set; }
        public string avatar { get; set; }


    }
}
