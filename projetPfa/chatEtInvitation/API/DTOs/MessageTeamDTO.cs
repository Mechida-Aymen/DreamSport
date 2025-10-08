namespace chatEtInvitation.API.DTOs
{
    public class MessageTeamDTO
    {
        public int? id {  get; set; }
        public int teamid {  get; set; }
        public int emetteur {  get; set; }
        public DateTime date { get; set; }
        public string contenue { get; set; }
        public int statutId { get; set; }
        public int AdminId { get; set; }

    }
}
