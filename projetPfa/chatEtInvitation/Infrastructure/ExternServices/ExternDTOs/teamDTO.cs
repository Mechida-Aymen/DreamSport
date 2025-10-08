namespace chatEtInvitation.Infrastructure.ExternServices.ExternDTOs
{
    public class teamDTO
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public int SportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int CaptainId { get; set; }

        public List<MembreDto>? Membres { get; set; }
    }

    public class MembreDto
    {
        public int UserId { get; set; }
        public int EquipeId { get; set; }
    }
}
