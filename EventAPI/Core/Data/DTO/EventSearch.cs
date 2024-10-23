namespace EventAPI.Core.Data.DTO
{
    public class EventSearch
    { 
        public EventSearch()
        {
            searches = new List<Search>();
        }
        public List<Search> searches { get; set; }
        public string Size { get; set; }
    }
    public class Search { 
    public string Type {  get; set; }
    public string Value { get; set; }

    }
}
