using EventAPI.Core.Data.Entities;
using EventAPI.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAPI.Core.Interfaces
{
    public interface IEventTicketMasterService
    {
        public Task<List<EventEntity>> GetEvents(string countryCode, string cityName);
        public Task<List<EventEntity>> GetAttraction(string cityName, string classificationName);
        public Task<List<EventEntity>> GetVenues(string countryCode, string keyword);

    }
}
