﻿namespace EventAPI.Core.Data.DTO
{
    public class SearchQuery
    { 
        public SearchQuery()
        {
            Search = new List<Search>();
        }
        public List<Search> Search { get; set; }
        public string Size { get; set; }
    }
    public class Search { 
    public string Type {  get; set; }
    public string Value { get; set; }

    }
}
