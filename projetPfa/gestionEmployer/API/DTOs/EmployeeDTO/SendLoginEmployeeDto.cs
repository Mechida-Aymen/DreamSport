namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    public class SendLoginEmployeeDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Image { get; set; }
        public string Role { get; set; } = "Employee";
    }
}
