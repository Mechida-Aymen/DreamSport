using gestionEquipe.Core.Models;

namespace gestionEquipe.API.DTOs
{
    public class UpdateEquipeDTO
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public String? Name { get; set; }
        public String? Description { get; set; }
        public String? Avatar { get; set; }
    }
}
