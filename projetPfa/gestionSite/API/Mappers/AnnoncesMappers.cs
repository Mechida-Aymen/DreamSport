using gestionSite.API.DTOs.AnnoncesDtos;
using gestionSite.API.DTOs.FAQDtos;
using gestionSite.Core.Models;
namespace gestionSite.API.Mappers

{
    public class AnnoncesMappers
    {
        public static Annonces AjouterAnnoncesDtoToAnnonces(AjouterAnnoncesDto ajouterAnnoncesDto)
        {
            return new Annonces
            {
                Description = ajouterAnnoncesDto.Description,
                LaunchedAt = ajouterAnnoncesDto.LaunchedAt,
                LifeTimeBySeconds = ajouterAnnoncesDto.LifeTimeBySeconds,
                IdAdmin = ajouterAnnoncesDto.AdminId,
            };

        }
    }
}
