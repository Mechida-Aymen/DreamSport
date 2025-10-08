using chatEtInvitation.Core.Models;
using chatEtInvitation.Infrastructure.Data.Repositories;
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

        public DbSet<AmisChat> AmisChats { get; set; }
        public DbSet<BloqueList> BloqueList { get; set; }
        public DbSet<ChatAmisMessage> ChatAmisMessages { get; set; }
        public DbSet<TeamChatMessage> TeamChatMessages { get; set; }
        public DbSet<MemberInvitation> MemberInvitations { get; set; }
        public DbSet<TeamInvitation> TeamInvitations { get; set; }
        public DbSet<TeamChat> TeamChats { get; set; }
        public DbSet<Statut> statuts { get; set; }
        public DbSet<MessageStatut> MessageStatuts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().UseTpcMappingStrategy();

            // Configure MessageStatut relationship
            modelBuilder.Entity<MessageStatut>(entity =>
            {
                entity.HasKey(ms => new { ms.MessageId, ms.StatutId, ms.UtilisateurId });

                entity.HasOne(ms => ms.Message)
                    .WithMany(m => m.Statuts)
                    .HasForeignKey(ms => ms.MessageId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ms => ms.Statut)
                    .WithMany(s => s.MessageStatuts)
                    .HasForeignKey(ms => ms.StatutId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Relationship: ChatAmisMessage -> AmisChat
            modelBuilder.Entity<ChatAmisMessage>()
                .HasOne(cam => cam._AmisChat)
                .WithMany()
                .HasForeignKey(cam => cam.ChatAmisId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: TeamChatMessage -> TeamChat
            modelBuilder.Entity<TeamChatMessage>()
                .HasOne(tcm => tcm._TeamChat)
                .WithMany()
                .HasForeignKey(tcm => tcm.TeamChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BloqueList>()
                .HasKey(bl => new { bl.Bloked, bl.BlokedBy });

            base.OnModelCreating(modelBuilder);
        }

    }
}

