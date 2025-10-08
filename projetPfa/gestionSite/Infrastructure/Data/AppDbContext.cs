using gestionSite.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionSite.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }   
        
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Terrain> Terrains { get; set; }
        public DbSet<TerrainStatus> TerrainStatuses { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Annonces> Annonces { get; set; }

        public DbSet<Sport_Categorie> Sports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Terrain>()
        .HasOne(t => t.Sport_Categorie)
        .WithMany()
        .HasForeignKey(t => t.IdSport_Categorie)
        .OnDelete(DeleteBehavior.Restrict); // Choose Restrict, SetNull, or Cascade

            // Correct foreign key relationship for TerrainStatus
            modelBuilder.Entity<Terrain>()
                .HasOne(t => t.terrainStatus)
                .WithMany()
                .HasForeignKey(t => t.TerrainStatusId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust as needed
        }

    }
}
