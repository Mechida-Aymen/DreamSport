using gestionEmployer.Core.Models;

namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    public class ReturnAddedEmployee : Employer
    {
        public Dictionary<string,string> errors { get; set; }= new Dictionary<string,string>();
    }
}
