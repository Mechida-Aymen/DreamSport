
namespace gestionEquipe.API.DTOs
{
    public class EquipeDto
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public int SportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int CaptainId { get; set; }

        public List<MembreDto>? Membres { get; set; }
        
    }
    }
