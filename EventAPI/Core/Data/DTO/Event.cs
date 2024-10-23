using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAPI.Core.Data.DTO
{
    public class Event
    {
        public string  Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }
}
