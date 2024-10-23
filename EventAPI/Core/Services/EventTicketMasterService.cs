using EventAPI.Core.Data.DTO;
using EventAPI.Core.Data.Entities;

using EventAPI.Core.Interfaces;
using EventAPI.Core.Data.Entities;
using EventAPI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Drawing;
using Namotion.Reflection;

namespace EventAPI.Core.Services
{
    public class EventTicketMasterService : IEventTicketMasterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _eventTicketMasterUrl;
        private readonly string _eventTicketMasterToken;
        private IConfiguration _configuration;
        private readonly ILogger<EventTicketMasterService> _logger;
        private string DefaultNoOfrecords;
        public EventTicketMasterService(HttpClient httpClient, IConfiguration configuration, ILogger<EventTicketMasterService> logger)

        {
            _httpClient = httpClient;
            _configuration = configuration;
            _eventTicketMasterUrl = configuration.GetRequiredSection("TicketMaster")["Url"];
            _eventTicketMasterToken = configuration.GetRequiredSection("TicketMaster")["APIKey"];
            _logger = logger;
            DefaultNoOfrecords= _configuration.GetSection("DefaultValues")["NoOfRecords"].ToString();
        }
        public async Task<List<EventEntity>> GetEvents(EventSearch eventSearch)
        {
           
            var result = new List<EventEntity>();
            var query = string.Empty;
            query = string.Join("", eventSearch.searches.Select(a => string.Concat("&",a.Type, "=", a.Value)));
            if(eventSearch.Size!="All")
            query += "&Size=" + (string.IsNullOrEmpty(eventSearch.Size) ? eventSearch.Size : DefaultNoOfrecords);

            var requestUrl = $"{_eventTicketMasterUrl}events.json?apikey={_eventTicketMasterToken}" + query;
            var response = await _httpClient.GetAsync(requestUrl);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize the response content into a JSON object (use your own models here)
                var ticketmasterEvents = JsonSerializer.Deserialize<TicketMasterResponse>(content);
                var events = ticketmasterEvents._embedded.events;
                result = events.Select(e => new EventEntity
                {
                    Id=Guid.NewGuid().ToString(),
                    Name = e.name,
                    Date = e.dates.start.dateTime,
                    Type = e.classifications[0].segment.name,
                    Url=e.url,
                    Source = "Ticketmaster",
                    Address = e.venues != null && e.venues.Count > 0 ? string.Concat(e.venues[0].address.line1, ",", e.venues[0].city.name, e.venues[0].country.name, e.venues[0].postalCode) : string.Empty
                }).ToList();
            }
            else
            {
                _logger.Log(LogLevel.Error, "Error accured fetching Event data from Event Service{SearchCriteria}", eventSearch);

                response.EnsureSuccessStatusCode();
            }
           
            return result;
        }

        public async Task<List<EventEntity>> GetAttraction(string cityName, string classificationName)
        {
            // not implemented
            return new List<EventEntity>();
        }
        public async Task<List<EventEntity>> GetVenues(string countryCode, string keyword)
        { 
            // not impelmented 
            return new List<EventEntity>(); }
    }

}
