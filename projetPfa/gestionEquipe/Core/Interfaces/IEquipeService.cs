using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;
using System.Collections.Generic;

namespace gestionEquipe.Core.Interfaces
{
    public interface IEquipeService
    {
        Task<bool> CapitainQuitteAsync(int capitaineId, int equipeId);
        Task<AddedEquipeDTO> AddEquipeAsync(Equipe _equipe);
        Task<UpdatedEquipeDTO> UpdateEquipeAsync(Equipe _equipe);
        Task<UpdatedEquipeDTO> TransferCaptaincyAsync(Equipe _equipe);

        // Méthode pour supprimer une équipe avec ses membres
        Task SupprimerEquipeAvecMembresAsync(int equipeId);
        Task<IEnumerable<int>> GetTeamAsync(int teamId);
        Task<UserTeamMembershipResponseDto> CheckUserTeamMembershipAsync(int userId, int adminId);
        Task<EquipeDto> GetEquipe(int IdEquipe);

    }
}
