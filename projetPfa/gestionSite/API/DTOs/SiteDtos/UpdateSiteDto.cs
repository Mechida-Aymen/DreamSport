namespace gestionSite.API.DTOs.SiteDtos
{
    public class UpdateSiteDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AboutUs { get; set; }
        public string? CouleurPrincipale { get; set; }
        public string? CouleurSecondaire { get; set; }
        public string? Background { get; set; }
        public string? Addresse { get; set; }
        public string? DomainName { get; set; }
        public int AdminId { get; set; }
    }
}
