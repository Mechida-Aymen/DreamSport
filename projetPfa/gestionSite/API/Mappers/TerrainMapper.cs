 using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.Core.Models;

namespace gestionSite.Infrastructure.Mappers
{
    public static class TerrainMapper
    {
        // Mapper pour l'ajout d'un terrain
        public static Terrain AddTerrainDtoToTerrain(AddTerrainDto addTerrainDto)
        {
            return new Terrain
            {
                Title = addTerrainDto.Title,
                Description = addTerrainDto.Description,
                Image = addTerrainDto.imageUrl,
                TerrainStatusId = addTerrainDto.TerrainStatusId,
                IdSport_Categorie = addTerrainDto.IdSport_Categorie,
                IdAdmin = addTerrainDto.AdminId
            };
        }

        // Mapper pour la mise à jour d'un terrain
        public static Terrain UpdateTerrainDtoToTerrain(UpdateTerrainDto updateTerrainDto)
        {
            return new Terrain
            {
                Id = updateTerrainDto.Id,
                Title = updateTerrainDto.Title,
                Description = updateTerrainDto.Description,
                Image = updateTerrainDto.Image,
                TerrainStatusId = updateTerrainDto.TerrainStatusId,
                IdAdmin = updateTerrainDto.AdminId,
            };
        }
    }
}
