using gestionEmployer.Core.Interfaces;
using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;

namespace gestionEmployer.Core.Services
{
    // PasswordService.cs
    public class PasswordService : IPasswordService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMailService _mailService;
        private readonly ISiteService _siteService;

        public PasswordService(IEmployeeRepository employeeRepository, IMailService mailService, ISiteService siteService)
        {
            _employeeRepository = employeeRepository;
            _mailService = mailService;
            _siteService = siteService;
        }

        public async Task<bool> VerifyOldPassword(int adminId, int employerId, string oldPassword)
        {
            // Récupération employé
            var employer = await _employeeRepository.GetEmployeeByIdAsync(employerId);

            // Vérifications
            if (employer == null)
                throw new KeyNotFoundException("Employé non trouvé");

            if (employer.AdminId != adminId)
                throw new UnauthorizedAccessException("Action non autorisée");

            // Vérification mot de passe (comparaison directe)
            return employer.Password == oldPassword;
        }

        public async Task ChangePassword(int adminId, int employerId, string newPassword)
        {
            // Vérification que l'admin a le droit (réutilise la même logique)
            await VerifyOldPassword(adminId, employerId, "dummy");
            // Le mot de passe dummy ne sera pas vérifié mais les autres checks le seront

            // Mise à jour du mot de passe
            var employer = await _employeeRepository.GetEmployeeByIdAsync(employerId);
            employer.Password = newPassword;

            await _employeeRepository.UpdateEmployeeAsync(employer);
            SiteDto site = await _siteService.GetSiteInfosAsync(employer.AdminId);
            if (site != null)
            {
                EmailRequest mail = new EmailRequest();
                mail.ChangePasswordMail(employer.Email, employer.Nom + " " + employer.Prenom, site.Name);
                await _mailService.NewEmployeeMail(mail, employer.AdminId);
            }
        }
    }
}
