namespace gestionUtilisateur.API.DTOs
{
    public class ReturnedLoginDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Image { get; set; }
        public string Role { get; set; } = "User";
    }
}
