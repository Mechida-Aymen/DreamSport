using Auth.Model;

namespace Auth.Interfaces
{
    public interface IValidateTokenBuilder
    {
        IValidateTokenBuilder SetUserId(int userId);
        IValidateTokenBuilder SetRole(string role);
        IValidateTokenBuilder SetAdminId(int adminId);
        IValidateTokenBuilder SetNom(string nom);
        IValidateTokenBuilder SetPrenom(string prenom);
        IValidateTokenBuilder SetImageUrl(string imageUrl);
        IValidateTokenBuilder SetCreatedAt(DateTime createdAt);
        IValidateTokenBuilder SetToken(string token);
        ValidateToken Build();
    }
}
