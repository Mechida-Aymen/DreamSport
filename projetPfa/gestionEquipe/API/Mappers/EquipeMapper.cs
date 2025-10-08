using gestionEquipe.API.DTOs;
using gestionEquipe.Core.Models;
using System.Reflection.Metadata.Ecma335;

namespace gestionEquipe.API.Mappers
{
    public class EquipeMapper
    {
        public static Equipe AddToModel(AddEquipeDTO dto)
        {
            return new Equipe
            {
                Name = dto.Name,
                AdminId = dto.AdminId,
                Description = dto.Description,
                Avatar = dto.Avatar,
                CaptainId = dto.CaptainId,
                SportId = dto.SportId,

            };
        }

        public static Equipe UpdateEquipDTOtoEquipe(UpdateEquipeDTO dto)
        {

            return new Equipe
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Avatar = dto.Avatar,
                SportId = dto.SportId,

            };
        }

        public static UpdatedEquipeDTO EquipetoUpdatedEquipeDTO(Equipe equipe)
        {
            if (equipe == null) return null;

            return new UpdatedEquipeDTO
            {
                Id = equipe.Id,
                CaptainId = equipe.CaptainId,
                Name = equipe.Name,
                SportId = equipe.SportId,
                Avatar=equipe.Avatar,
                Description= equipe.Description,
                Members = equipe.Members,
            };
        }


        public static AddedEquipeDTO ModelToAdded(Equipe equipe)
        {
            return new AddedEquipeDTO
            {
                CaptainId = equipe.CaptainId,
                Name = equipe.Name,
                Description = equipe.Description,
                Avatar = equipe.Avatar,
                AdminId = equipe.AdminId,
                SportId= equipe.SportId,
            };
        }
        public static Equipe ChangerCapitainDTOToEquipe(ChangerCapitaineEquipeDTO dto)
        {
            return new Equipe
            {
                Id=dto.idEquipe,
               
                CaptainId=dto.idCapitain,
    
            };
        }
        public static UpdatedEquipeDTO EquipeChangedCapitain(Equipe dto)
        {
            return new UpdatedEquipeDTO
            {
                Id = dto.Id,
                Name = dto.Name,
                CaptainId = dto.CaptainId,
                Description = dto.Description,
                Avatar = dto.Avatar,
                SportId = dto.SportId,
                Errors=new Dictionary<string, string>()
            };
        }
    

    }
}
