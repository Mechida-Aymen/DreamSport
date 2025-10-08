using gestionReservation.Infrastructure.ExternServices.Extern_DTo;

namespace gestionReservation.Core.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(EmailRequest request, int adminId);
    }
}
