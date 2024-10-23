using EventAPI.Core.Data.DTO;
using EventAPI.Core.Data.Entities;
using EventAPI.Core.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAPI.Core.Interfaces
{
    // Services/IEventService.cs
    public interface IEventService
    {
        public Task FetchAndStoreEventsAsync(EventSearch eventSearch); 
        public Task<IEnumerable<Data.DTO.Event>> GetAllEvents();
        public Task<string> PostYourEvent(string postDescription);

    }

}
