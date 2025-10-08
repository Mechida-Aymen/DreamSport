namespace chatEtInvitation.API.DTOs
{
    public class MessageWithStatutsDto
    {
        public int MessageId { get; set; }
        public string Contenu { get; set; }
        public int EmetteurId { get; set; }
        public List<StatutDto> Statuts { get; set; }
    }

    public class StatutDto
    {
        public int StatutId { get; set; }
        public string Libelle { get; set; }
    }
}
