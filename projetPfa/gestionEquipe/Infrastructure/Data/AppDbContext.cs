using gestionEquipe.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace gestionEquipe.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Equipe> Equipes { get; set; }
        
        public DbSet<Members> Memberss { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite primary key
            modelBuilder.Entity<Members>()
                .HasKey(m => new { m.UserId, m.EquipeId });

            // Configure the foreign key to Equipe, no foreign key to User since it's in a different service
            modelBuilder.Entity<Members>()
                .HasOne(m => m.Equipe)
                .WithMany(e => e.Members)
                .HasForeignKey(m => m.EquipeId)
                .OnDelete(DeleteBehavior.Cascade);  // Ajout de la suppression en cascade
        }


    }
}
