using gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs;

namespace gestionUtilisateur.Core.Interfaces
{
    public interface IMailService
    {
        Task<bool> MailRecoverkey(EmailRequest request,int adminId);
    }
}
