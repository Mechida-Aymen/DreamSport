namespace gestionUtilisateur.Core.Interfaces
{
    public interface IPasswordUserService
    {
        Task<bool> VerifyOldUserPassword(int IdAdmin, int userId, string oldPassword);
        Task ChangeUserPassword(int IdAdmin, int userId, string newPassword);
    }
}
