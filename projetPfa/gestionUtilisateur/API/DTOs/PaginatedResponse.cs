namespace gestionUtilisateur.API.DTOs
{
    public class PaginatedResponse<T>
    {
        public List<T> users { get; set; }
        public int TotalCount { get; set; }

        public PaginatedResponse(List<T> items, int totalCount)
        {
            users = items;
            TotalCount = totalCount;
        }
    }
}
