using CartonCaps.Referrals.Api.Middlewares;
using CartonCaps.Referrals.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CartonCaps.Referrals.UnitTests.Middlewares;

[TestFixture]
public class UserContextMiddlewareTests
{
    private Mock<RequestDelegate> _mockNext;
    private Mock<IUserContext> _mockUserContext;
    private UserContextMiddleware _middleware;
    private DefaultHttpContext _httpContext;

    [SetUp]
    public void Setup()
    {
        _mockNext = new Mock<RequestDelegate>();
        _mockUserContext = new Mock<IUserContext>();
        _middleware = new UserContextMiddleware(_mockNext.Object);
        _httpContext = new DefaultHttpContext();
    }

    [Test]
    public async Task InvokeAsync_WhenHeaderPresentWithValidUserId_ShouldSetUserContextUserId()
    {
        // Arrange
        const long expectedUserId = 12345;
        _httpContext.Request.Headers["X-User-Id"] = expectedUserId.ToString();

        // Act
        await _middleware.InvokeAsync(_httpContext, _mockUserContext.Object);

        // Assert
        _mockUserContext.VerifySet(x => x.UserId = expectedUserId, Times.Once);
        _mockNext.Verify(n => n(_httpContext), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenHeaderPresentWithInvalidUserId_ShouldNotSetUserContextUserId()
    {
        // Arrange
        _httpContext.Request.Headers["X-User-Id"] = "invalid";

        // Act
        await _middleware.InvokeAsync(_httpContext, _mockUserContext.Object);

        // Assert
        _mockUserContext.VerifySet(x => x.UserId = It.IsAny<long>(), Times.Never);
        Assert.That(_mockUserContext.Object.IsAuthenticated, Is.False);
        _mockNext.Verify(n => n(_httpContext), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenHeaderNotPresent_ShouldNotSetUserContextUserId()
    {
        // Arrange
        // No header added to the context

        // Act
        await _middleware.InvokeAsync(_httpContext, _mockUserContext.Object);

        // Assert
        _mockUserContext.VerifySet(x => x.UserId = It.IsAny<long>(), Times.Never);
        Assert.That(_mockUserContext.Object.IsAuthenticated, Is.False);
    }

    [Test]
    public async Task InvokeAsync_WhenHeaderContainsMaxLongValue_ShouldSetUserContextCorrectly()
    {
        // Arrange
        var expectedUserId = long.MaxValue;
        _httpContext.Request.Headers["X-User-Id"] = expectedUserId.ToString();

        // Act
        await _middleware.InvokeAsync(_httpContext, _mockUserContext.Object);

        // Assert
        _mockUserContext.VerifySet(x => x.UserId = expectedUserId, Times.Once);
        _mockNext.Verify(n => n(_httpContext), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenHeaderContainsValueOutOfLongRange_ShouldNotSetUserContextUserId()
    {
        // Arrange
        _httpContext.Request.Headers["X-User-Id"] = "9223372036854775808"; // long.MaxValue + 1

        // Act
        await _middleware.InvokeAsync(_httpContext, _mockUserContext.Object);

        // Assert
        _mockUserContext.VerifySet(x => x.UserId = It.IsAny<long>(), Times.Never);
        _mockNext.Verify(n => n(_httpContext), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenHeaderContainsEmptyString_ShouldNotSetUserContextUserId()
    {
        // Arrange
        _httpContext.Request.Headers["X-User-Id"] = "";

        // Act
        await _middleware.InvokeAsync(_httpContext, _mockUserContext.Object);

        // Assert
        _mockUserContext.VerifySet(x => x.UserId = It.IsAny<long>(), Times.Never);
        _mockNext.Verify(n => n(_httpContext), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenHeaderContainsWhitespace_ShouldNotSetUserContextUserId()
    {
        // Arrange
        _httpContext.Request.Headers["X-User-Id"] = " ";

        // Act
        await _middleware.InvokeAsync(_httpContext, _mockUserContext.Object);

        // Assert
        _mockUserContext.VerifySet(x => x.UserId = It.IsAny<long>(), Times.Never);
        _mockNext.Verify(n => n(_httpContext), Times.Once);
    }
}