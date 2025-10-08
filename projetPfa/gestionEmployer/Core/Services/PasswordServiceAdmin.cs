using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Infrastructure.Data.Repositories;
using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;

namespace gestionEmployer.Core.Services
{
    // PasswordService.cs
    public class PasswordServiceAdmin : IPasswordServiceAdmin
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMailService _mailService;
        private readonly ISiteService _siteService;

        public PasswordServiceAdmin(IAdminRepository adminRepository, IMailService mailService, ISiteService siteService)
        {
            _adminRepository = adminRepository;
            _mailService = mailService;
            _siteService = siteService; 
        }

        public async Task<bool> VerifyOldPasswordAdmin(int adminId, string oldPassword)
        {
            // Récupération employé
            var Admin = _adminRepository.GetAdminByTenantId(adminId);

            // Vérifications
            if (Admin == null)
                throw new KeyNotFoundException("Admin non trouvé");


            // Vérification mot de passe (comparaison directe)
            return Admin.Password == oldPassword;
        }

        public async Task ChangePasswordAdmin(int adminId, string newPassword)
        {
            // Vérification que l'admin a le droit (réutilise la même logique)
            await VerifyOldPasswordAdmin(adminId, "dummy");
            // Le mot de passe dummy ne sera pas vérifié mais les autres checks le seront

            // Mise à jour du mot de passe
            var Admin =  _adminRepository.GetAdminByTenantId(adminId);
            Admin.Password = newPassword;

            await _adminRepository.UpdateAdminAsync(Admin);
            SiteDto site = await _siteService.GetSiteInfosAsync(Admin.Id);
            if (site != null)
            {
                EmailRequest mail = new EmailRequest();
                mail.ChangePasswordMail(Admin.Email, Admin.Nom + " " + Admin.Prenom, site.Name);
                await _mailService.NewEmployeeMail(mail, Admin.Id);
            }

        }
    }
}
