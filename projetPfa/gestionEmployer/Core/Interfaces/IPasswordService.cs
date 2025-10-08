namespace gestionEmployer.Core.Interfaces
{
    public interface IPasswordService
    {
        Task<bool> VerifyOldPassword(int adminId, int employerId, string oldPassword);
        Task ChangePassword(int adminId, int employerId, string newPassword);
    }
}
