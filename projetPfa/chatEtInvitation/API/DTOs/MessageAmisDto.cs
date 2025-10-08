namespace chatEtInvitation.API.DTOs
{
    public class MessageAmisDto
    {
        public int? id { get; set; }
        public int recepteur { get; set; }
        public int emetteur { get; set; }
        public DateTime date { get; set; }
        public string contenue { get; set; }
        public int statutId { get; set; }
        public int AdminId { get; set; }

    }
}
