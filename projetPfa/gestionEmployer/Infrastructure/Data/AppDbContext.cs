using gestionEmployer.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace gestionEmployer.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employer> Employers { get; set; }
        public DbSet<EmployerStatus> EmployerStatuss { get; set; }
        public DbSet<Admin> Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship: Employer has a foreign key to Admin
        modelBuilder.Entity<Employer>()
            .HasOne<Admin>()
            .WithMany()
            .HasForeignKey(e => e.AdminId)
            .OnDelete(DeleteBehavior.Restrict); // You can specify behavior like Restrict, Cascade, etc.
    }

    }
}
