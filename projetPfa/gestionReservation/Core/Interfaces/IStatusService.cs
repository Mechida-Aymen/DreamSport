using gestionReservation.Core.Models;

public interface IStatusService
{
    // Récupérer un statut par son ID
    Task<Status> GetStatusByIdAsync(int statusId);

    // Récupérer le statut "En attente" par défaut
    Task<Status> GetDefaultStatusAsync();
}
