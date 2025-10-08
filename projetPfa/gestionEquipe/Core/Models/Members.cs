namespace gestionEquipe.Core.Models
{
    public class Members
    {
        public int UserId { get; set; }
        public int EquipeId { get; set; }
        public Equipe Equipe { get; set; }
    }
}
