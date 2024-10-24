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
using Microsoft.Extensions.Primitives;


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
        public EventService(IEventTicketMasterService eventTicketMasterService,IAyrshare _ayrshareServic, IMemoryCache cache,
            IConfiguration configuration,
            IAyrshare ayrshareService, ILogger<EventService> logger)

        {
            _cache = cache;
            _eventTicketMasterService = eventTicketMasterService;
            _configuration = configuration;
            _ayrshareService = ayrshareService;
            _logger = logger;
            LoadData();
        }
        #region Data from MasterTicket
        public async Task FetchAndStoreEventsAsync(SearchQuery eventQuery)
        {

            var filePath = _configuration["Storage:FilePath"];
            
            if (!File.Exists(filePath + "events.json"))
                File.Create(filePath + "events.json");
           
            var api1Events = await _eventTicketMasterService.GetEvents(eventQuery);
            _eventList.AddRange(api1Events);
            
            string json = JsonSerializer.Serialize(_eventList);
            File.WriteAllText(filePath+"events.json", json);
            if(_eventList != null && _eventList.Count>0)
            {
                var allevent = _eventList.Select(e => new Data.DTO.Event()
                {
                    Id=e.Id,
                    Name = e.Name,
                    Type = e.Type,
                 
                    url=e.Url,
                    Date = e.Date

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
            LoadData();
            
            return _eventList.Select(e => new Data.DTO.Event()
            {
                Id = e.Id,
                Name = e.Name,
                Type = e.Type,
                Date = e.Date

            });
        }
        private void LoadData()
        {
            if (_cache.TryGetValue(CacheKey, out List<Data.DTO.Event> cachedEvents))
            {
                 return;
            }
            string jsonString = File.ReadAllText(@"Core\Data\DB\events.json");

            // Deserialize the JSON string into a list of Event objects
            List<EventEntity> allevent = JsonSerializer.Deserialize<List<EventEntity>>(jsonString);
            _eventList.AddRange(allevent);
            cachedEvents= _eventList.Select(e => new Data.DTO.Event()
            {
                Id = e.Id,
                Name = e.Name,
                Type = e.Type,
                Address = e.Address,
                url = e.Url,
                Date = e.Date

            }).ToList();
            _cache.Set(CacheKey, cachedEvents, TimeSpan.FromMinutes(30));

        }
        public async Task<Data.DTO.Event> GetEvent(string eventID)
        {
            var eventDTO = new Data.DTO.Event();
            if (_cache.TryGetValue(CacheKey, out List<Data.DTO.Event> cachedEvents) && cachedEvents!=null)
            {
                return cachedEvents.FirstOrDefault(e => e.Id == eventID);
            }
            if (_eventList != null)
            {
                eventDTO = _eventList.Where(e => e.Id == eventID).Select(e => new Data.DTO.Event()
                {
                    Name = e.Name,
                    Type = e.Type,
                   
                    Date = e.Date,
                   
                }).FirstOrDefault();
            }
            return eventDTO;
        }
            #endregion
            #region Ayshare
            public async Task<string> PostYourEvent(EventPost eventPost)
            {

                var Selectedevent = GetEvent(eventPost.EventID).Result;
                string result = string.Empty;
                if (Selectedevent != null)
                {
                    StringBuilder description = new StringBuilder();
                    description.Append("Name:"+Selectedevent?.Name);
                    description.Append(Environment.NewLine);
                    description.Append("Type"+Selectedevent?.Type);
                    description.Append(Environment.NewLine);
                    description.Append(eventPost.PostDescription);
                    description.Append(Environment.NewLine);
                    description.Append("Link:"+Selectedevent?.url);
                    description.Append(Selectedevent?.Date.ToString("dd-mm-yyyy"));
                    return await _ayrshareService.PostYourEvent(description.ToString());
                }
                return result;
            }
            #endregion
        }

}
