using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EventAPI.Core.Data.DTO;
using EventAPI.Core.Data.Entities;
using EventAPI.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using Microsoft.Extensions.Logging;


namespace EventAPI.Core.Services
{


    public class EventService : IEventService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "events";
        private readonly List<EventEntity> _eventList = new();
        private IEventTicketMasterService _eventTicketMasterService;
        private IAyrshare _ayrshareService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventService> _logger;
        public EventService(IEventTicketMasterService eventTicketMasterService, IMemoryCache cache,
            IConfiguration configuration,
            IAyrshare ayrshareService, ILogger<EventService> logger)

        {
            _cache = cache;
            _eventTicketMasterService = eventTicketMasterService;
            _configuration = configuration;
            _ayrshareService = ayrshareService;
            _logger = logger;
        }
        #region Data from MasterTicket
        public async Task FetchAndStoreEventsAsync(EventSearch eventSearch)
        {

            var filePath = _configuration["Storage:FilePath"];
            
            if (!File.Exists(filePath + "events.json"))
                File.Create(filePath + "events.json");
           
            var api1Events = await _eventTicketMasterService.GetEvents(eventSearch.CountryCode, eventSearch.City);
            _eventList.AddRange(api1Events);
            
            string json = JsonSerializer.Serialize(_eventList);
            File.WriteAllText(filePath+"events.json", json);
            if(_eventList != null && _eventList.Count>0)
            {
                var allevent = _eventList.Select(a => new Data.DTO.Event()
                {
                    Name = a.Name,
                    Type = a.Type,
                    CountryCode = a.CountryCode,
                    Address = a.Address,
                    Date = a.Date

                });
                _cache.Set(CacheKey, allevent, TimeSpan.FromMinutes(30));
                _logger.Log(LogLevel.Information, "Successfully fetch the data{_eventList}", _eventList);
            }
            else
            {
                _logger.Log(LogLevel.Information, "Successfully fetch the data{_eventList}", _eventList);
            }
           
          
        }
        #endregion
        #region Event Local storage
        public async Task<IEnumerable<Data.DTO.Event>> GetAllEvents()
        {
            if (_cache.TryGetValue(CacheKey, out List<Data.DTO.Event> cachedEvents))
            {
                return cachedEvents;
            }
            string jsonString = File.ReadAllText(@"Core\Data\DB\events.json");

            // Deserialize the JSON string into a list of Event objects
            List<EventEntity> allevent = JsonSerializer.Deserialize<List<EventEntity>>(jsonString);
            _eventList.AddRange(allevent);
          return _eventList.Select(e => new Data.DTO.Event()
            {
                Id = e.Id,
                Name = e.Name,
                Type = e.Type,
                CountryCode = e.CountryCode,
                Address = e.Address,
                Date = e.Date

            });
        }
        public async Task<Data.DTO.Event> GetEvent(string eventId)
        {
            var eventDTO= new Data.DTO.Event();
            if (_eventList != null)
            {
                eventDTO = _eventList.Where(e => e.Id == eventId).Select(e => new Data.DTO.Event()
                {
                    Name = e.Name,
                    Type = e.Type,
                    CountryCode = e.CountryCode,
                    Address = e.Address,
                    Date = e.Date,
                    Description = e.Description,
                    City = e.City
                }).FirstOrDefault();
            }
            return eventDTO;
        }
            #endregion
            #region Ayshare
            public async Task PostEvent(string EventID)
        {

            var Selectedevent = _eventList.FirstOrDefault(e => e.Id == EventID);


            if (Selectedevent != null)
            {
                StringBuilder description = new StringBuilder();
                description.Append(Selectedevent?.Name);
                description.Append(Selectedevent?.Description);
                description.Append(Selectedevent?.Address);
                description.Append(Selectedevent?.Date.ToString("dd-mm-yyyy"));
                string response = _ayrshareService.PostYourEvent(description.ToString()).Result;
                return response;
            }
            return null;

        }
            #endregion
        }

}
