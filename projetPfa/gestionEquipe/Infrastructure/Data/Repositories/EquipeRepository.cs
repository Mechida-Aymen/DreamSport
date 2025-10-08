using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionEquipe.Infrastructure.Data.Repositories
{
    public class EquipeRepository : IEquipeRepository
    {
        private readonly AppDbContext _context; 

        public EquipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Equipe> AddEquipeAsync(Equipe _equipe)
        {
                var result = await _context.Equipes.AddAsync(_equipe);
                await _context.SaveChangesAsync();

                return result.Entity;
            
        }

        public async Task<Equipe> UpdateEquipeAsync(Equipe _equipe)
        {
           

              _context.Equipes.Update(_equipe);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return _equipe; 
           
        }


        public async Task<int> CountEquipesBySportAndUser(int userId, int SportId)
        {
            return await _context.Equipes
                .Where(e => e.SportId == SportId && e.Members.Any(m => m.UserId == userId))
                .CountAsync();
        }

        public async Task<bool> ExistWithName(string name, int AdminID)
        {
            return await _context.Equipes.AnyAsync(e => e.Name == name && e.AdminId == AdminID);

        }

        public async Task<bool> IsCaptainAsync(int CaptainID, int EquipeId)
        {
            return await _context.Equipes.AnyAsync(e => e.CaptainId == CaptainID && e.Id == EquipeId);
        }
        
        public async Task<Equipe> ExistWithIdAsync(int id)
        {
            return await _context.Equipes
                                     .FirstOrDefaultAsync(m => m.Id == id);
        }

        //supprimer equipe with id (membres inclus grace oncascade)

        // Méthode pour supprimer l'équipe et ses membres
        public async Task SupprimerEquipeAvecMembresAsync(int equipeId)
        {
            var equipe = await _context.Equipes
                .Include(e => e.Members)  // Inclure les membres pour supprimer en cascade
                .FirstOrDefaultAsync(e => e.Id == equipeId);

            if (equipe == null)
            {
                throw new Exception("Équipe non trouvée");
            }

            // Supprimer l'équipe (les membres seront supprimés automatiquement grâce à la cascade)
            _context.Equipes.Remove(equipe);
            await _context.SaveChangesAsync();
        }



        public async Task<Equipe> GetEquipeById(int id)
        {
           
                var equipe = await _context.Equipes
                    .FirstOrDefaultAsync(e => e.Id == id);


                return equipe;
           
        }

        //-------------
        public async Task<Members> GetUserTeamMembershipAsync(int userId, int adminId)
        {
            return await _context.Memberss
                .Include(m => m.Equipe) // Inclure les détails de l'équipe
                .FirstOrDefaultAsync(m => m.UserId == userId && m.Equipe.AdminId == adminId);
        }


    }

}
