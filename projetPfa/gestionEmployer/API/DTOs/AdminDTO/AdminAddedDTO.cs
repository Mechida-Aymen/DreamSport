namespace gestionEmployer.API.DTOs.AdminDTO
{
    public class AdminAddedDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }

        public List<string> errors { get; set; } = new List<string>();

    }
}
