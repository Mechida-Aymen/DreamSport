using Auth.Interfaces;
using Auth.Model;

namespace Auth.Models
{
    public class ValidateTokenBuilder : IValidateTokenBuilder
    {
        private readonly ValidateToken _token = new ValidateToken();

        public IValidateTokenBuilder SetUserId(int userId)
        {
            _token.UserId = userId;
            return this;
        }

        public IValidateTokenBuilder SetRole(string role)
        {
            _token.Role = role;
            return this;
        }

        public IValidateTokenBuilder SetAdminId(int adminId)
        {
            _token.AdminId = adminId;
            return this;
        }

        public IValidateTokenBuilder SetNom(string nom)
        {
            _token.Nom = nom;
            return this;
        }

        public IValidateTokenBuilder SetPrenom(string prenom)
        {
            _token.Prenom = prenom;
            return this;
        }

        public IValidateTokenBuilder SetImageUrl(string imageUrl)
        {
            _token.ImageUrl = imageUrl;
            return this;
        }

        public IValidateTokenBuilder SetCreatedAt(DateTime createdAt)
        {
            _token.CreatedAt = createdAt;
            return this;
        }

        public IValidateTokenBuilder SetToken(string token)
        {
            _token.Token = token;
            return this;
        }

        public ValidateToken Build()
        {
            return _token;
        }
    }

}
