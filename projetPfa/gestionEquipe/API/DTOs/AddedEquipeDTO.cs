namespace gestionEquipe.API.DTOs
{
    public class AddedEquipeDTO 
    {
        public int AdminId { get; set; }
        public int SportId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String? Avatar { get; set; }
        public int CaptainId { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

        public List<DeleteMemberDTO> Members { get; set; }

    }
}
