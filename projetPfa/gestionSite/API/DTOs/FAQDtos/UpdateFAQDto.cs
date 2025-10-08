using System.ComponentModel.DataAnnotations;

namespace gestionSite.API.DTOs.FAQDtos
{
    public class UpdateFAQDto
    {
        public int Id { get; set; }

        public string? Question { get; set; }
        public string? Response { get; set; }
        public int AdminId { get; set; }

    }
}
