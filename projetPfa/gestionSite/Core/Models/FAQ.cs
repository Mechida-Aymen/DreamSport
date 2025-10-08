namespace gestionSite.Core.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public int IdAdmin { get; set; }
    }
}
