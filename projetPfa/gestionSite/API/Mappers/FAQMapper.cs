using gestionSite.API.DTOs.FAQDtos;
using gestionSite.Core.Models;

namespace gestionSite.API.Mappers
{
    public class FAQMapper
    {
        public static FAQ AddFAQDtoToFAQ (AddFAQDto addFAQDto)
        {
            return new FAQ
            {
                Question = addFAQDto.Question,
                Response = addFAQDto.Response,
                IdAdmin = addFAQDto.AdminId,
            };
        }

        public static FAQ UpdateFAQDtoToFAQ(UpdateFAQDto UpdateFAQDto)
        {
            return new FAQ
            {
                Id = UpdateFAQDto.Id,
                Question = UpdateFAQDto.Question,
                Response = UpdateFAQDto.Response,
                IdAdmin = UpdateFAQDto.AdminId
            };
        }
    }
}
