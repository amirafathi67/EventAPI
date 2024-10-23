
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
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        // Arrange
        var username = "admin";
        var password = "admin";
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("SomeSecretKey");

        // Act
        var result = await _controller.Login(username, password) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<string>(result.Value);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var username = "user";
        var password = "wrongpassword";

        // Act
        var result = await _controller.Login(username, password);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
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
        var eventSearch = new EventSearch();
        eventSearch.searches.Add(new Search() { Type = "Countrycode", Value = "IE" });
        eventSearch.searches.Add(new Search() { Type = "city", Value = "Dublin" });
        eventSearch.Size = "10";
        // Act
        var result = await _controller.FetchEvents(eventSearch) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Events fetched and stored successfully.", result.Value);
    }

    [Fact]
    public async Task PostYourEvent_ReturnsOk_WhenEventIsPostedSuccessfully()
    {
        // Arrange
        var eventId = "12345";

        // Act
        var result = await _controller.PostYourEvent(eventId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Events have been post successfully.", result.Value);
    }
}
