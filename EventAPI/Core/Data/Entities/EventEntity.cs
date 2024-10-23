using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAPI.Core.Data.Entities
{
    // Models/EventEntity.cs
    public class EventEntity
    {
        public EventEntity()
        {
            venues = new List<venues>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Url { get; set; }
        public List<venues> venues { set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime Date { get; set; }

        internal void Select(Func<object, DTO.Event> value)
        {
            throw new NotImplementedException();
        }
    }
    public class venues
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string address { get; set; }

                                                                                                                                                                
    }
}
