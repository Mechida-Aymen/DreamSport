using Auth.Interfaces;
using Auth.Model;
using Microsoft.EntityFrameworkCore;

namespace Auth
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext context;

        public TokenRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ValidateToken> GetValidateTokenAsync(string token)
        {
            return await context.validateTokens.FirstOrDefaultAsync(v => v.Token == token);
        }

        public async Task<ValidateToken> AddTokenAsync(ValidateToken token)
        {
            await context.validateTokens.AddAsync(token);
            await context.SaveChangesAsync(); 

            return token;
        }

        public async Task<ValidateToken> UpdateTokenAsync(ValidateToken token)
        {
            var tokensToUpdate = await context.validateTokens
                .Where(t => t.UserId == token.UserId && t.Role == token.Role)
                .ToListAsync();

            if (!tokensToUpdate.Any())
            {
                throw new KeyNotFoundException("No tokens found for the given user and role.");
            }

            foreach (var t in tokensToUpdate)
            {
                t.Nom = token.Nom;
                t.Prenom = token.Prenom;
                t.ImageUrl = token.ImageUrl;
            }

            await context.SaveChangesAsync();

            return token;
        }

    }
}
