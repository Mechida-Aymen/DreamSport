
using gestionUtilisateur.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionUtilisateur.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }   
        
        public DbSet<User> Users { get; set; }
        

    }
}
