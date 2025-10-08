using Auth.Model;

namespace Auth.Interfaces
{
    public interface ITokenRepository
    {
        Task<ValidateToken> AddTokenAsync(ValidateToken token);
        Task<ValidateToken> GetValidateTokenAsync(string token);
        Task<ValidateToken> UpdateTokenAsync(ValidateToken token);
    }
}
