using gestionEquipe.Core.Models;

namespace gestionEquipe.Core.Interfaces
{
    public interface IEquipeRepository
    {
        Task<Equipe> AddEquipeAsync(Equipe _equipe);
        Task<int> CountEquipesBySportAndUser(int userId, int SportId);
        Task<bool> ExistWithName(string name, int AdminID);
        Task<Equipe> ExistWithIdAsync(int id);
        Task<bool> IsCaptainAsync(int CaptainID, int EquipeId);
        Task<Equipe> UpdateEquipeAsync(Equipe _equipe);
        Task<Equipe> GetEquipeById(int id);
        //supprimer equipe with members by id
        // Méthode pour supprimer une équipe et ses membres
        Task SupprimerEquipeAvecMembresAsync(int equipeId);
        Task<Members> GetUserTeamMembershipAsync(int userId, int adminId);


    }
}
