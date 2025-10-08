using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Infrastructure.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.API.Mappers;
using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;


namespace gestionEmployer.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMailService _mailService;
        private readonly IAdminRepository _adminRepository;
        private readonly IAuthService _authService;

        public EmployeeService(IEmployeeRepository employeeRepository, IMailService mailService, IAdminRepository adminRepository, IAuthService authService)
        {
            _employeeRepository = employeeRepository;
            _mailService = mailService;
            _adminRepository = adminRepository;
            _authService = authService;
        }

        public async Task<Employer> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return employee;
        }


        //Methode de recuperations de tous les employees by IdAdmin

        public async Task<IEnumerable<GetEmployeeDTO>> GetEmployesByAdminIdAsync(int AdminId)
        {
            IEnumerable<Employer> empList = await _employeeRepository.GetEmployesByAdminIdAsync(AdminId);
            IEnumerable<GetEmployeeDTO> dtoList = empList.Select(e => EmployeeMapper.ModelToGetEmployee(e));
            return dtoList;

        }

        private string GenererNouveauMotDePasse()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ReturnAddedEmployee> AddEmployeeAsync(Employer employee)
        {
            ReturnAddedEmployee _ReturnAddedEmployee = EmployeeMapper.EmployeeToRTE(employee);
            if (_employeeRepository.Exists(e => e.CIN == employee.CIN && e.AdminId == employee.AdminId))
                _ReturnAddedEmployee.errors.Add("CIN", "CIN already exists");

            if (_employeeRepository.Exists(e => e.Email == employee.Email && e.AdminId == employee.AdminId))
                _ReturnAddedEmployee.errors.Add("Email", "Email already exists");

            if (_employeeRepository.Exists(e => e.Username == employee.Username && e.AdminId == employee.AdminId))
                _ReturnAddedEmployee.errors.Add("Username", "Username already exists");

            if (_employeeRepository.Exists(e => e.PhoneNumber == employee.PhoneNumber && e.AdminId == employee.AdminId))
                _ReturnAddedEmployee.errors.Add("PhoneNumber", "PhoneNumber already exists");

            if (_ReturnAddedEmployee.errors.Count > 0)
            {
                return _ReturnAddedEmployee;
            }

            // si toutes les attributs ne se répete pas enregistre l'employé 
            employee.Password = GenererNouveauMotDePasse();
            Employer emp = await _employeeRepository.AddEmployeeAsync(employee);
            EmailRequest emailRequest = new EmailRequest(employee.Email, employee.Nom + " " + employee.Prenom, employee.Password);
            var xd = await _mailService.NewEmployeeMail(emailRequest, emp.AdminId);
            return _ReturnAddedEmployee;

        }


        //Dans cette mettre on vas modifier l'employé en verifiant tous les attributs s'ils existent déja
        public async Task<ReturnUpdatedEmpDto?> UpdateEmployeeAsync(Employer updatedEmploye)
        {
            // Récupérer l'employé existant
            Employer existingEmploye = await _employeeRepository.GetEmployeeByIdAsync(updatedEmploye.Id)
                                    ?? throw new KeyNotFoundException("Employé non trouvé.");

            // Liste pour stocker les erreurs trouvées
            ReturnUpdatedEmpDto dto = EmployeeMapper.ModelToUpdate(updatedEmploye);

            // Vérification de l'unicité seulement si l'attribut est modifié
            if (updatedEmploye.CIN != null && !updatedEmploye.CIN.Equals(existingEmploye.CIN))
            {
                if (await _employeeRepository.EmployerByCINAsync(updatedEmploye.CIN, updatedEmploye.AdminId) != null)
                {
                    {
                        dto.Errors.Add("CIN", "CIN already exist.");
                    }
                }
            }
            if (updatedEmploye.Email != null && !updatedEmploye.Email.Equals(existingEmploye.Email))
            {
                if (await _employeeRepository.EmployerByEmailAsync(updatedEmploye.Email, updatedEmploye.AdminId) != null)
                {
                    dto.Errors.Add("Email", "Email already exist..");
                }
            }
            if (updatedEmploye.Username != null && !updatedEmploye.Username.Equals(existingEmploye.Username))
            {
                if (await _employeeRepository.EmployerByUsernameAsync(updatedEmploye.Username, updatedEmploye.AdminId) != null)
                {
                    dto.Errors.Add("Username", "Username already exist.");
                }
            }
            if (updatedEmploye.PhoneNumber != null && !updatedEmploye.PhoneNumber.Equals(existingEmploye.PhoneNumber))
            {
                if (await _employeeRepository.EmployerByPhoneAsync(updatedEmploye.PhoneNumber, updatedEmploye.AdminId) != null)
                {
                    dto.Errors.Add("PhoneNumber", "PhoneNumber already exist.");
                }
            }

            if (dto.Errors.Count() == 0)
            {
                EmployeeMapper.updateToModel(existingEmploye, updatedEmploye);
                // Sauvegarder les changements dans la base de données
                var empp = await _employeeRepository.UpdateEmployeeAsync(existingEmploye);
                SendLoginEmployeeDto tk = new SendLoginEmployeeDto
                {
                    Id = empp.Id,
                    Nom = empp.Nom,
                    Prenom = empp.Prenom,
                    Image = empp.imageUrl
                };
                await _authService.UpdateTokenAsync(tk, empp.AdminId);
            }

            return dto;
        }


        public async Task<Employer> DeleteEmployeeAsync(int id)
        {
            //Vérification si l'employé existe  sinon il retourne un message "Employé non trouvé"
            var existingEmploye = _employeeRepository.GetEmployeeByIdAsync(id)
                                      ?? throw new KeyNotFoundException("Employé non trouvé.");

            //Suppression de l'employé qui a id entré
            return await _employeeRepository.DeleteEmployeeAsync(id);
        }


        public async Task<Employer> ModifyProfileAsync(Employer employer)
        {
            // Récupérer l'employé avec l'ID fourni
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employer.Id);

            // Vérifier si l'employé existe
            if (employee != null)
            {
                // Mettre à jour les informations de l'employé
                employee.Nom = employer.Nom;
                employee.Prenom = employer.Prenom;
                employee.Email = employer.Email;
                employee.PhoneNumber = employer.PhoneNumber;

                // Appeler la méthode pour mettre à jour l'employé dans la base de données
                await _employeeRepository.UpdateEmployeeAsync(employee);

                // Retourner l'employé mis à jour
                return employee;
            }

            // Si l'employé n'est pas trouvé, retourner null ou lever une exception selon ton besoin
            return null;
        }


        public async Task<SendLoginEmployeeDto> ValidateLogin(EmployerLoginDto login)
        {
            Employer employer = await _employeeRepository.EmployerByEmailAsync(login.Email, login.AdminId);
            if (employer == null)
            {
                throw new KeyNotFoundException("The employee dont exist");
            }
            if (!employer.Password.Equals(login.Password))
            {
                return null;
            }
            return EmployeeMapper.ModelToLogin(employer);
        }

        public async Task<IEnumerable<GetEmployeeDTO>> SearchEmployeesAsync(string searchTerm)
        {
            List<Employer> empList = await _employeeRepository.SearchEmployeesAsync(searchTerm);
            IEnumerable<GetEmployeeDTO> dtoList = empList.Select(e => EmployeeMapper.ModelToGetEmployee(e));

            // Mapper manuellement les entités vers les DTOs
            return dtoList;
        }

        // change password 
        public async Task<bool> VerifyOldPassword(int adminId, int employerId, string oldPassword)
        {
            var employer = await _employeeRepository.GetEmployeeByIdAsync(employerId);

            if (employer == null || employer.AdminId != adminId)
            {
                throw new UnauthorizedAccessException("Droits insuffisants ou employé introuvable");
            }

            // ATTENTION: Comparaison directe (non sécurisée)
            return employer.Password == oldPassword;
        }

        public async Task<ReturnForgotPasswordDTO> RecupererPasswodAsync(recoverPass dto)
        {
            // Recherche l'utilisateur par email
            Employer user = await _employeeRepository.EmployerByEmailAsync(dto.Email, dto.AdminId);
            if (user == null)
            {
                var Returnto = EmployeeMapper.recoverTOreturn(dto);
                Returnto.error = "Aucun utilisateur trouvé avec cet email";
                return Returnto;
            }
            var ReturnDto = EmployeeMapper.recoverTOreturn(dto);

            // Générer un nouveau mot de passe
            var nouveauMotDePasse = GenererNouveauMotDePasse();

            // Mise à jour du mot de passe dans l'objet utilisateur
            user.Password = nouveauMotDePasse;

            // Mise à jour dans la base de données
            await _employeeRepository.UpdateEmployeeAsync(user);

            EmailRequest emailRequest = new EmailRequest(user.Email, nouveauMotDePasse, user.Nom , user.Prenom);
            await _mailService.NewEmployeeMail(emailRequest,user.AdminId);
            // Retourner true après une mise à jour réussie
            return ReturnDto;
        }
    }
}
