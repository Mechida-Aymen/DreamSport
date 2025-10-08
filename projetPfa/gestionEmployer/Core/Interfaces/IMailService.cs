using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;

namespace gestionEmployer.Core.Interfaces
{
    public interface IMailService
    {
        Task<bool> NewEmployeeMail(EmailRequest requestn ,int adminId);
    }
}
