using gestionSite.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace gestionSite.Core.Interfaces.SportInterfaces
{
    public interface ISportService
    {
        Task<IEnumerable<Sport_Categorie>> GetSportsAsync();
        
    }
}
