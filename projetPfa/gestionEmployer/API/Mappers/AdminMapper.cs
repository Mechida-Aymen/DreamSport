using gestionEmployer.API.DTOs.AdminDTO;
using gestionEmployer.API.DTOs.DTOs;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.Core.Models;

namespace gestionEmployer.API.Mappers
{
    public class AdminMapper
    {
        public static Admin AdminDTOToAdmin(AjouterAdminDTO dto)
        {
            return new Admin
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Login = dto.Login
            };
        }

        public static AdminAddedDTO AdminToAdminAddedDTO(Admin admin)
        {
            return new AdminAddedDTO
            {
                Id = admin.Id,
                Nom = admin.Nom,
                Prenom = admin.Prenom,
                Login = admin.Login
            };
        }

        public static SendLoginEmployeeDto ModelToLogin(Admin _Employer)
        {
            return new SendLoginEmployeeDto
            {
                Id = _Employer.Id,
                Nom = _Employer.Nom,
                Prenom = _Employer.Prenom,
            };
        }

        public static Admin UpdateAdminDTOToAdmin(UpdateAdminDTO _UpdateAdminDTO)
        {
            return new Admin
            {
                Id = _UpdateAdminDTO.Id,
                Nom = _UpdateAdminDTO.Nom,
                Prenom = _UpdateAdminDTO.Prenom,
                Login = _UpdateAdminDTO.Login,
                PhoneNumber = _UpdateAdminDTO.PhoneNumber,
                Email = _UpdateAdminDTO.Email,
            };
        }

        public static void updateToModel(Admin existingAdmin, Admin updatedAdmin)
        {
            if (!string.IsNullOrEmpty(updatedAdmin.Nom))
            {
                existingAdmin.Nom = updatedAdmin.Nom;
            }
            if (!string.IsNullOrEmpty(updatedAdmin.Prenom))
            {
                existingAdmin.Prenom = updatedAdmin.Prenom;
            }
            if (!string.IsNullOrEmpty(updatedAdmin.Login))
            {
                existingAdmin.Login = updatedAdmin.Login;
            }
            if (!string.IsNullOrEmpty(updatedAdmin.PhoneNumber))
            {
                existingAdmin.PhoneNumber = updatedAdmin.PhoneNumber;
            }

        }


        public static ReturnUpdatedAdminDto ModelToUpdate(Admin _Admin)
        {
            return new ReturnUpdatedAdminDto
            {
                Nom = _Admin.Nom,
                Prenom = _Admin.Prenom,
                PhoneNumber = _Admin.PhoneNumber,
                Login = _Admin.Login,
                Id = _Admin.Id,
                Email = _Admin.Email,
            };
        }

        public static ReturnAdminDto ModeltoReturn(Admin _Admin)
        {
            return new ReturnAdminDto
            {
                Id = _Admin.Id,
                Nom = _Admin.Nom,
                Prenom = _Admin.Prenom,
                PhoneNumber = _Admin.PhoneNumber,
                Login = _Admin.Login,
                Email = _Admin.Email,
            };
        }
    }
}
