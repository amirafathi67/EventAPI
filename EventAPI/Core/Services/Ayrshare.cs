using EventAPI.Core.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace EventAPI.Core.Services
{
    public class AyrshareService : IAyrshare
    {
        private readonly HttpClient _httpClient;
        private readonly string _ayrshareUrl;
        private readonly string _ayrshareToken;
        private IConfiguration _configuration;
        private readonly ILogger<AyrshareService> _logger;
        public AyrshareService(HttpClient httpClient, IConfiguration configuration, ILogger<AyrshareService> logger)

        {
            _httpClient = httpClient;
            _configuration = configuration;
            _ayrshareUrl = configuration.GetRequiredSection("Ayrshare")["Url"];
            _ayrshareToken = configuration.GetRequiredSection("Ayrshare")["APIKey"];
            _logger = logger;
        }
        public async Task<string> PostYourEvent(string postDescription)
        {

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+ _ayrshareToken);

            var data = new
            {   post = postDescription,
                platforms = new[] { "twitter", "facebook", "instagram", "linkedin", "pinterest" },
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_ayrshareUrl, content);
            if (response==null || !response.IsSuccessStatusCode)
            {
                _logger.LogError("Error getting Event from Ayshare service: {responseString}", response);
                return null;
            }
            var result = await response.Content.ReadAsStringAsync();

            return response.StatusCode.ToString();

        }
    }
}
