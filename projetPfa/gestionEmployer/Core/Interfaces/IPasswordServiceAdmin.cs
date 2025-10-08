namespace gestionEmployer.Core.Interfaces
{
    public interface IPasswordServiceAdmin
    {
        Task<bool> VerifyOldPasswordAdmin(int adminId, string oldPassword);
        Task ChangePasswordAdmin(int adminId, string newPassword);
    }
}
