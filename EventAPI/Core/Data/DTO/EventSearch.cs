namespace EventAPI.Core.Data.DTO
{
    public class EventSearch
    {
        public int Id { get; set; }
        
        public int? Page { get; set; }
        public int? Size { get; set; }
    }
}
