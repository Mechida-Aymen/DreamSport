using gestionReservation.Core.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet pour les réservations
    public DbSet<Reservation> Reservations { get; set; }

   

    // DbSet pour les statuts
    public DbSet<Status> Status { get; set; }

    // Autres entités comme Employe, Admin, etc. selon votre modèle

    // Exemple de configuration spécifique
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurer la relation entre Reservation et Status (Many-to-One)
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Status)
            .WithMany()
            .HasForeignKey(r => r.IdStatus)
            .OnDelete(DeleteBehavior.SetNull); // Vous pouvez ajuster le comportement de suppression ici

        // Si vous avez des relations supplémentaires, ajoutez-les ici
    }
}
