using Auth.Interfaces;

namespace Auth.Model
{
    public class ValidateTokenDirector
    {
        private readonly IValidateTokenBuilder _builder;

        // Constructor that accepts the builder
        public ValidateTokenDirector(IValidateTokenBuilder builder)
        {
            _builder = builder;
        }

        // A method that defines the "recipe" for constructing the ValidateToken object
        public ValidateToken BuildUserToken(int userId, string role, int adminId, string nom, string prenom, string imageUrl, DateTime createdAt, string token)
        {
            return _builder
                .SetUserId(userId)
                .SetRole(role)
                .SetAdminId(adminId)
                .SetNom(nom)
                .SetPrenom(prenom)
                .SetImageUrl(imageUrl)
                .SetCreatedAt(createdAt)
                .SetToken(token)
                .Build();
        }

        public ValidateToken BuildEmpToken(int userId, string role, int adminId, string nom, string prenom, DateTime createdAt, string token, string imageUrl)
        {
            return _builder
                .SetUserId(userId)
                .SetRole(role)
                .SetAdminId(adminId)
                .SetNom(nom)
                .SetPrenom(prenom)
                .SetCreatedAt(createdAt)
                .SetToken(token)
                .SetImageUrl(imageUrl)
                .Build();
        }
    }

}
