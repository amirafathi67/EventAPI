
using EventAPI.Core.Data.DTO;
using EventAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class EventsControllerTests
{
    private readonly Mock<IEventService> _mockEventService;
    private readonly Mock<ILogger<EventsController>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly EventsController _controller;

    public EventsControllerTests()
    {
        _mockEventService = new Mock<IEventService>();
        _mockLogger = new Mock<ILogger<EventsController>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _controller = new EventsController(_mockEventService.Object, _mockLogger.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task GetAllEvents_ReturnsOk_WithEventList()
    {
        // Arrange
        _mockEventService.Setup(es => es.GetAllEvents()).ReturnsAsync(new List<EventAPI.Core.Data.DTO.Event>());

        // Act
        var result = await _controller.GetAllEvents() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<List<Event>>(result.Value);
    }

    [Fact]
    public async Task FetchEvents_ReturnsOk_WhenFetchIsSuccessful()
    {
        // Arrange
        var searchQuery = new SearchQuery();
        searchQuery.Search.Add(new Search() { Type = "Countrycode", Value = "IE" });
        searchQuery.Search.Add(new Search() { Type = "city", Value = "Dublin" });
        searchQuery.Size = "10";
        // Act
        var result = await _controller.FetchEvents(searchQuery) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Events fetched and stored successfully.", result.Value);
    }

    [Fact]
    public async Task PostYourEvent_ReturnsOk_WhenEventIsPostedSuccessfully()
    {
        // Arrange
        var eventPost = new EventPost() { EventID = "12345", PostDescription = "This Event is for learning style importance in Education" };
         

        // Act
        var result = await _controller.PostYourEvent(eventPost) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Events have been post successfully.", result.Value);
    }
}
