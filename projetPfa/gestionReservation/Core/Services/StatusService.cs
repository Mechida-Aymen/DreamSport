using gestionReservation.Core.Models;
using Microsoft.EntityFrameworkCore;

public class StatusService : IStatusService
{
    private readonly AppDbContext _context;  // Suppose que vous avez un DbContext pour l'accès à la base de données

    public StatusService(AppDbContext context)
    {
        _context = context;
    }

    // Récupérer un statut par ID
    public async Task<Status> GetStatusByIdAsync(int statusId)
    {
        var status = await _context.Status.FindAsync(statusId);

        // Si aucun statut n'est trouvé, retourner null ou gérer autrement
        if (status == null)
        {
            // Optionnel : loggez ou gérez l'erreur ici
            return null;
        }

        return status;
    }


    // Récupérer le statut "En attente" par défaut
    public async Task<Status> GetDefaultStatusAsync()
    {
        return await _context.Status.FirstOrDefaultAsync(s => s.Libelle == "En attente");
    }

}
