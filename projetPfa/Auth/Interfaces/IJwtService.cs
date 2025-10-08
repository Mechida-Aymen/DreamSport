using Auth.Model;
using System.Security.Claims;

namespace Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(int userId, string role, int AdminId, string Nom, string Prenom, string imageUrl);
        Task<string> GenerateRefreshToken();
        Task<ValidateToken> updateTokenAsync(ValidateToken token);
    }
}
