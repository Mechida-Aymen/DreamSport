using Auth.Model;
using Microsoft.EntityFrameworkCore;

namespace Auth
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ValidateToken> validateTokens { get; set; }
        
    }
}
