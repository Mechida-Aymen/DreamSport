using gestionEmployer.API.DTOs.DTOs;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.Core.Models;

namespace gestionEmployer.API.Mappers
{
    public class EmployeeMapper
    {
        public static Employer AddEmployeeDTOToEmployer(AddEmployeeDTO _AddEmployeeDTO)
        {
            return new Employer
            {
                AdminId = _AddEmployeeDTO.AdminId,
                Nom = _AddEmployeeDTO.Nom,
                Prenom = _AddEmployeeDTO.Prenom,
                Birthday = _AddEmployeeDTO.Birthday,
                PhoneNumber = _AddEmployeeDTO.PhoneNumber,
                CIN = _AddEmployeeDTO.CIN,
                Email = _AddEmployeeDTO.Email,
                Username = _AddEmployeeDTO.Username,
                Salaire = _AddEmployeeDTO.Salaire,
                imageUrl = _AddEmployeeDTO.ImageUrl,    
            };
        }
        public static Employer UpdateEmployeeDTOToEmployer(UpdateEmployeeDTO _UpdateEmployeeDTO)
        {
            return new Employer
            {
                Id = _UpdateEmployeeDTO.Id,
                Nom = _UpdateEmployeeDTO.Nom,
                Prenom = _UpdateEmployeeDTO.Prenom,
                Birthday = _UpdateEmployeeDTO.Birthday,
                PhoneNumber = _UpdateEmployeeDTO.PhoneNumber,
                Email = _UpdateEmployeeDTO.Email,
                Username = _UpdateEmployeeDTO.Username,
                Salaire = _UpdateEmployeeDTO.Salaire,
                imageUrl = _UpdateEmployeeDTO.imageUrl,
                AdminId = _UpdateEmployeeDTO.AdminId,
            };
        }

        public static void updateToModel(Employer existingEmploye, Employer updatedEmploye)
        {
            if (!string.IsNullOrEmpty(updatedEmploye.Email))
            {
                existingEmploye.Email = updatedEmploye.Email;
            }
            if (!string.IsNullOrEmpty(updatedEmploye.Username))
            {
                existingEmploye.Username = updatedEmploye.Username;
            }
            if (!string.IsNullOrEmpty(updatedEmploye.Nom))
            {
                existingEmploye.Nom = updatedEmploye.Nom;
            }
            if (!string.IsNullOrEmpty(updatedEmploye.Prenom))
            {
                existingEmploye.Prenom = updatedEmploye.Prenom;
            }
            if (!string.IsNullOrEmpty(updatedEmploye.PhoneNumber))
            {
                existingEmploye.PhoneNumber = updatedEmploye.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(updatedEmploye.imageUrl))
            {
                existingEmploye.imageUrl = updatedEmploye.imageUrl;
            }
            if (updatedEmploye.Salaire != null && updatedEmploye.Salaire != 0.0)
            {
                existingEmploye.Salaire = updatedEmploye.Salaire;
            }
            if (updatedEmploye.Birthday != null && updatedEmploye.Birthday != default(DateTime))
            {
                existingEmploye.Birthday = updatedEmploye.Birthday;
            }
        }

        public static ReturnAddedEmployee EmployeeToRTE(Employer _Employer)
        {
            return new ReturnAddedEmployee
            {
                Id = _Employer.Id,
                Nom = _Employer.Nom,
                Prenom = _Employer.Prenom,
                Birthday = _Employer.Birthday,
                PhoneNumber = _Employer.PhoneNumber,
                Email = _Employer.Email,
                Username = _Employer.Username,
                Salaire = _Employer.Salaire,
                AdminId = _Employer.AdminId,
                imageUrl = _Employer.imageUrl,
            };
        }

        public static GetEmployeeDTO ModelToGetEmployee(Employer _Employer)
        {
            return new GetEmployeeDTO
            {
                Id = _Employer.Id,
                AdminId = _Employer.AdminId,
                Nom = _Employer.Nom,
                Prenom = _Employer.Prenom,
                CIN = _Employer.CIN,
                Email = _Employer.Email,
                Birthday = _Employer.Birthday,
                PhoneNumber = _Employer.PhoneNumber,
                Salaire = _Employer.Salaire,
                Username = _Employer.Username,
                imageUrl = _Employer.imageUrl, 
            };
        }

        public static ReturnUpdatedEmpDto ModelToUpdate(Employer _Employer)
        {
            return new ReturnUpdatedEmpDto
            {
                Nom = _Employer.Nom,
                Prenom = _Employer.Prenom,
                Email = _Employer.Email,
                Birthday = _Employer.Birthday,
                PhoneNumber = _Employer.PhoneNumber,
                Salaire = _Employer.Salaire,
                Username = _Employer.Username,
                Id = _Employer.Id
            };
        }
    
        public static SendLoginEmployeeDto ModelToLogin(Employer _Employer)
        {
            return new SendLoginEmployeeDto
            {
                Id = _Employer.Id,
                Nom = _Employer.Nom,
                Prenom = _Employer.Prenom,
                Image = _Employer.imageUrl
            };
        }

        public static ReturnForgotPasswordDTO recoverTOreturn(recoverPass dto)
        {
            return new ReturnForgotPasswordDTO
            {
                Email = dto.Email,
                AdminId = dto.AdminId,
            };
        }
    }
}
