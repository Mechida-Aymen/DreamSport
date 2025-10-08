namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    public class ReturnForgotPasswordDTO
    {
        public string Email { get; set; }
        public int AdminId { get; set; }
        public string? error { get; set; }
    }
}
