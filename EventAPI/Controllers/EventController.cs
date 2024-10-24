// Controllers/EventsController.cs
using EventAPI.Core.Data.DTO;
using EventAPI.Core.Interfaces;

using EventAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventsController> _logger;
    private readonly IConfiguration _configuration;
    public EventsController(IEventService eventService, ILogger<EventsController> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _eventService = eventService;
        _logger = logger;

    }

    [HttpPost]
    [Route("FetchEvents")]
    public async Task<IActionResult> FetchEvents([FromBody] SearchQuery eventSearch)
    {
        try
        {
            await _eventService.FetchAndStoreEventsAsync(eventSearch);
            return Ok("Events fetched and stored successfully.");
        }
        catch (Exception ex)
        {
            const string message = "Error occured event.";
            _logger.LogError(ex, message);
            throw new Exception(message);
        }
    }

    [HttpGet]
    [Route("GetAllEvents")]
    public async Task<IActionResult> GetAllEvents()
    {
        try
        {
            var events = await _eventService.GetAllEvents();
            return Ok(events);
        }
        catch (Exception ex)
        {
            const string message = "Error occured event.";
            _logger.LogError(ex, message);
            throw new Exception(message);
        }
    }

    [HttpPost]
    [Route("PostEvent")]
    public async Task<IActionResult> PostYourEvent(EventPost eventPost)
    {
        try
        {
            await _eventService.PostYourEvent(eventPost);
            return Ok("Events have been post successfully.");
        }
        catch (Exception ex)
        {
            const string message = "Error occured event.";
            _logger.LogError(ex, message);
            throw new Exception(message);
        }
    }
}

