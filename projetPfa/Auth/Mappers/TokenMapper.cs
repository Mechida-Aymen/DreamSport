using Auth.Dtos;
using Auth.Model;

namespace Auth.Mappers
{
    public class TokenMapper
    {
        public static ValidateToken updateToModel(UpdateTokenDto dto)
        {
            return new ValidateToken
            {
                UserId = dto.Id,
                AdminId = dto.AdminId,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                ImageUrl = dto.Image,
                Role = dto.Role
            };
        }
    }
}
