using gestionEmployer.API.DTOs.AdminDTO;
using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.API.Mappers;
using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Interfaces.CasheInterfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Infrastructure.Data.Repositories;
using gestionEmployer.Infrastructure.ExternServices;
using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;
using Shared.Messaging.Services;

namespace gestionEmployer.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly ICacheService _cacheService;
        private readonly IAdminRepository _adminRepository;
        private readonly IMailService _mailService;

        public AdminService(IAdminRepository adminRepository, IMailService mailService, ICacheService cacheService)
        {
            _adminRepository = adminRepository;
            _mailService = mailService;
            _cacheService = cacheService;
        }

        public bool ValidateTenant(int tenantId)
        {
            Admin admin = this.GetAdmin(tenantId);
            if(admin == null)
            {
                return false;
            }
            return true;
        }

        public Admin? GetAdmin(int tenantId)
        {
            string cacheKey = $"admin:{tenantId}";

            Admin admin = _cacheService.Get<Admin>(cacheKey);
            if (admin == null)
            {
                // If not found in cache, fetch from DB
                admin = _adminRepository.GetAdminByTenantId(tenantId);

                if(admin != null){
                _cacheService.Set(cacheKey, admin, TimeSpan.FromMinutes(5));  // Set TTL as needed
                }
            }

            return admin;
        }
        public  AdminAddedDTO AjouterAdmin(Admin admin)
        {
            AdminAddedDTO _adminDTO = AdminMapper.AdminToAdminAddedDTO(admin);

            if (_adminRepository.AdminExists(admin.Nom, admin.Login, admin.PhoneNumber))
            {
                _adminDTO.errors.Add("An administrator with this name, login, or phone number already exists");
                return _adminDTO;
            }

            var adminAdded = _adminRepository.AddAdmin(admin);

            if (adminAdded == null)
            {
                _adminDTO.errors.Add("An error occurred while adding the administrator. Please try again.");
                return _adminDTO;
            }
            EmailRequest mail = new EmailRequest();
            mail.SendNewAdminWelcomeEmail(admin.Email, admin.Nom + " " + admin.Prenom, admin.Login, admin.Password);
            _mailService.NewEmployeeMail(mail, adminAdded.Id);

            _adminDTO = AdminMapper.AdminToAdminAddedDTO(adminAdded);

            var _producer = new RabbitMQProducerService("Creation de site");
            // Utilisation de RabbitMQ Producer injecté
            _producer.Publish(new { Id = _adminDTO.Id, Name = _adminDTO.Nom });

            return _adminDTO;
        }

        public async Task<SendLoginEmployeeDto> ValidateLoginAsync(AdminLoginDto dto)
        {
            Admin admin = await _adminRepository.GetByLoginAsync(dto.Login, dto.AdminId);
            if(admin == null)
            {
                throw new KeyNotFoundException("Tenant not Found");
            }
            if (!admin.Password.Equals(dto.Password))
            {
                return null;
            }
            return AdminMapper.ModelToLogin(admin);
        }

        //public async Task<ReturnUpdatedAdminDto?> UpdateAdminAsync(Admin updatedAdmin)
        // {
        //     // Récupérer l'employé existant
        //     Admin existingAdmin = _adminRepository.GetAdminByTenantId(updatedAdmin.Id)
        //                             ?? throw new KeyNotFoundException("Admin non trouvé.");

        //     // Liste pour stocker les erreurs trouvées
        //     ReturnUpdatedAdminDto dto = AdminMapper.ModelToUpdate(updatedAdmin);

        //     // Vérification de l'unicité seulement si l'attribut est modifié
        //     if (updatedAdmin.Login != null && !updatedAdmin.Login.Equals(existingAdmin.Login))
        //     {
        //         if (await _adminRepository.GetByLoginAsync(updatedAdmin.Login, updatedAdmin.Id) != null)
        //         {
        //             {
        //                 dto.Errors.Add("Login", "Login already exist.");
        //             }
        //         }
        //     }
        //     if (updatedAdmin.PhoneNumber != null && !updatedAdmin.PhoneNumber.Equals(existingAdmin.PhoneNumber))
        //     {
        //         if (await _adminRepository.EmployerByPhoneAsync(updatedAdmin.PhoneNumber, updatedAdmin.Id) != null)
        //         {
        //             dto.Errors.Add("PhoneNumber", "PhoneNumber already exist.");
        //         }
        //     }

        //     if (dto.Errors.Count() == 0)
        //     {
        //         EmployeeMapper.updateToModel(existingEmploye, updatedEmploye);
        //         // Sauvegarder les changements dans la base de données
        //         var empp = await _employeeRepository.UpdateEmployeeAsync(existingEmploye);
        //     }

        //     return dto;
        // }
        public async Task<ReturnForgotPasswordDTO> RecupererPasswodAsync(recoverPass dto)
        {
            // Recherche l'utilisateur par email
            Admin user = this.GetAdmin(dto.AdminId);
            if (user == null || !user.Email.Equals(dto.Email))
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
            Admin result = await _adminRepository.UpdateAdminAsync(user);
            if(result == null)
            {
                return ReturnDto;
            }
            _cacheService.Remove($"admin:{dto.AdminId}");

            EmailRequest emailRequest = new EmailRequest(user.Email, nouveauMotDePasse, user.Nom, user.Prenom);
            await _mailService.NewEmployeeMail(emailRequest, user.Id);
            // Retourner true après une mise à jour réussie
            return ReturnDto;
        }

        private string GenererNouveauMotDePasse()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<ReturnUpdatedAdminDto?> UpdateAdminAsync(Admin admin)
        {
            // Récupérer l'employé existant
            Admin existingAdmin = this.GetAdmin(admin.Id)
                                    ?? throw new KeyNotFoundException("Admin non trouvé.");

            // Liste pour stocker les erreurs trouvées
            ReturnUpdatedAdminDto dto = AdminMapper.ModelToUpdate(existingAdmin);

            // Vérification de l'unicité seulement si l'attribut est modifié
            if (admin.Email != null && !admin.Email.Equals(existingAdmin.Email))
            {
                if (await _adminRepository.GetAdminByEmailAsync(admin.Email, admin.Id) != null)
                {
                    {
                        dto.Errors.Add("Email", "Email already exist.");
                    }
                }
            }
            if (admin.PhoneNumber != null && !admin.PhoneNumber.Equals(existingAdmin.PhoneNumber))
            {
                if (await _adminRepository.GetAdminByPhoneAsync(admin.PhoneNumber, admin.Id) != null)
                {
                    {
                        dto.Errors.Add("PhoneNumber", "PhoneNumber already exist.");
                    }
                }
            }
            if (admin.Login != null && !admin.Login.Equals(existingAdmin.Login))
            {
                if (await _adminRepository.GetByLoginAsync(admin.Login, admin.Id) != null)
                {
                    {
                        dto.Errors.Add("Login", "Login already exist.");
                    }
                }
            }
            

            if (dto.Errors.Count() == 0)
            {
                admin.Password = existingAdmin.Password;
                Admin empp = await _adminRepository.UpdateAdminAsync(admin);
                if(empp != null)
                {
                    _cacheService.Remove($"admin:{admin.Id}");
                }
            }

            return dto;
        }

        public async Task<ReturnAdminDto?> GetADminByIdAsync(int id)
        {
            string cacheKey = $"admin:{id}";

            Admin admin = await _cacheService.GetAsync<Admin>(cacheKey);
            if (admin == null)
            {
                // If not found in cache, fetch from DB
                admin = await _adminRepository.GetAdminAsync(id);

                if(admin!=null){
                await _cacheService.SetAsync(cacheKey, admin, TimeSpan.FromMinutes(5));  // Set TTL as needed
                }
            }
            if (admin == null)
            {
                return null;
            }
            ReturnAdminDto dto = AdminMapper.ModeltoReturn(admin);
            return dto;
           
        }

    }


}
