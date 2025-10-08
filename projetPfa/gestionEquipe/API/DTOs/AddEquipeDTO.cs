namespace gestionEquipe.API.DTOs
{
    public class AddEquipeDTO
    {
        public int AdminId { get; set; }
        public int SportId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String? Avatar { get; set; }
        public int CaptainId { get; set; }
    }
}
