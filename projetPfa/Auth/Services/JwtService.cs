using Auth.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Auth.Model;

namespace Auth.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly int _tokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;  // Added for refresh token expiration time
        private readonly string _issuer;
        private readonly string _audience;

        private readonly ITokenRepository _tokenRepository;

        public JwtService(IConfiguration configuration, ITokenRepository tokenRepository)
        {
            _secretKey = configuration["Jwt:Secret"];
            _tokenExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
            _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"] ?? "30");  // Refresh token expiration time
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _tokenRepository = tokenRepository;
        }

        // Generate Access Token (Short-lived)
        public string GenerateAccessToken(int userId, string role, int AdminId, string Nom, string Prenom, string imageUrl)
        {
            var claims = GetClaims(userId, role, AdminId, Nom, Prenom, imageUrl);
            var key = GetSigningKey();
            var tokenDescriptor = GetTokenDescriptor(claims, key);
            
            return CreateToken(tokenDescriptor);
        }

        // Generate Refresh Token (Long-lived)
        public async Task<string> GenerateRefreshToken()
        {
            int maxAttempts = 10; // Limit the number of attempts to avoid infinite loops
            int attempts = 0;
            string token;
            do
            {
                var randomNumber = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                }
                token = Convert.ToBase64String(randomNumber);

                attempts++;
                if (attempts >= maxAttempts)
                {
                    return null;
                }

            } while (await _tokenRepository.GetValidateTokenAsync(token) != null); 
            return token;
        }

        // Generate the claims for the token
        private Claim[] GetClaims(int userId, string role, int AdminId, string Nom, string Prenom, string imageUrl)
        {
            if (imageUrl == null)
            {
                return new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim("Role", role),
                    new Claim("AdminId", AdminId.ToString()),
                    new Claim("Nom", Nom),
                    new Claim("Prenom", Prenom)
                };
            }
            return new[]
           {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("Role", role),
                new Claim("AdminId", AdminId.ToString()),
                new Claim("Nom", Nom),
                new Claim("Prenom", Prenom),
                new Claim("ImageUrl", imageUrl)
            };

        }

        // Create a signing key from the secret key
        private SymmetricSecurityKey GetSigningKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }

        // Get token descriptor
        private SecurityTokenDescriptor GetTokenDescriptor(Claim[] claims, SymmetricSecurityKey key)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),  // Access token expiration time
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
        }

        // Create token
        private string CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<ValidateToken> updateTokenAsync(ValidateToken token)
        {
            return await _tokenRepository.UpdateTokenAsync(token);
        }
    }
}
