using CartonCaps.Referrals.Api.Controllers;
using CartonCaps.Referrals.Services.Enums;
using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Models.Referral;
using CartonCaps.Referrals.Services.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CartonCaps.Referrals.UnitTests.Controllers;

[TestFixture]
public class ReferralsControllerTests
{
    private Mock<IUserContext> _mockUserContext;
    private Mock<IReferralService> _mockReferralService;
    private Mock<IUserService> _mockUserService;
    private ReferralsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockUserContext = new Mock<IUserContext>();
        _mockReferralService = new Mock<IReferralService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new ReferralsController(
            _mockUserContext.Object,
            _mockReferralService.Object,
            _mockUserService.Object);
    }

    [TearDown]
    public void Teardown()
    {
        _controller.Dispose();
    }

    #region GetForCurrentUser Tests

    [Test]
    public async Task GetForCurrentUser_WhenUserIsAuthenticated_ShouldReturnOkWithReferrals()
    {
        // Arrange
        const long userId = 123;
        var expectedResponse = new ReferralListResponse
        {
            ReferrerUserId = userId,
            Data = [
                new() { ReferredUserId = 456 }
            ]
        };
        var result = new GenericResult<ReferralListResponse>(expectedResponse);

        _mockUserContext.Setup(x => x.IsAuthenticated).Returns(true);
        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockReferralService.Setup(x => x.GetReferralsForUser(userId)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetForCurrentUser();

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)actionResult;
        Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
        _mockReferralService.Verify(x => x.GetReferralsForUser(userId), Times.Once);
    }

    [Test]
    public async Task GetForCurrentUser_WhenUserIsNotAuthenticated_ShouldReturnUnauthorized()
    {
        // Arrange
        _mockUserContext.Setup(x => x.IsAuthenticated).Returns(false);

        // Act
        var actionResult = await _controller.GetForCurrentUser();

        // Assert
        Assert.That(actionResult, Is.InstanceOf<UnauthorizedObjectResult>());
        var unauthorizedResult = (UnauthorizedObjectResult)actionResult;
        Assert.That(unauthorizedResult.Value, Is.EqualTo("User is not authenticated"));
        _mockReferralService.Verify(x => x.GetReferralsForUser(It.IsAny<long>()), Times.Never);
    }

    [Test]
    public async Task GetForCurrentUser_WhenServiceReturnsError_ShouldReturnBadRequest()
    {
        // Arrange
        const long userId = 123;
        var result = new GenericResult<ReferralListResponse>("Error occurred", ErrorCode.GenericError);

        _mockUserContext.Setup(x => x.IsAuthenticated).Returns(true);
        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockReferralService.Setup(x => x.GetReferralsForUser(userId)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetForCurrentUser();

        // Assert
        Assert.That(actionResult, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)actionResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Error occurred"));
    }

    #endregion

    #region GetByUserId Tests

    [Test]
    public async Task GetByUserId_WhenUserExists_ShouldReturnOkWithReferrals()
    {
        // Arrange
        const long userId = 456;
        var expectedResponse = new ReferralListResponse
        {
            ReferrerUserId = userId,
            Data = [
                new() { ReferredUserId = 456 }
            ]
        };
        var result = new GenericResult<ReferralListResponse>(expectedResponse);

        _mockReferralService.Setup(x => x.GetReferralsForUser(userId)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetByUserId(userId);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)actionResult;
        Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
        _mockReferralService.Verify(x => x.GetReferralsForUser(userId), Times.Once);
    }

    [Test]
    public async Task GetByUserId_WhenServiceReturnsNotFound_ShouldReturnNotFound()
    {
        // Arrange
        const long userId = 999;
        var result = new GenericResult<ReferralListResponse>("User not found", ErrorCode.NotFound);

        _mockReferralService.Setup(x => x.GetReferralsForUser(userId)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetByUserId(userId);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = (NotFoundObjectResult)actionResult;
        Assert.That(notFoundResult.Value, Is.EqualTo("User not found"));
    }

    [Test]
    public async Task GetByUserId_WhenServiceReturnsError_ShouldReturnBadRequest()
    {
        // Arrange
        const long userId = 123;
        var result = new GenericResult<ReferralListResponse>("Error occurred", ErrorCode.GenericError);

        _mockReferralService.Setup(x => x.GetReferralsForUser(userId)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetByUserId(userId);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<BadRequestObjectResult>());
    }

    #endregion

    #region CreateReferral Tests

    [Test]
    public async Task CreateReferral_WhenValidRequest_ShouldReturnCreatedWithReferral()
    {
        // Arrange
        var request = new CreateReferralRequest
        {
            ReferrerUserId = 123,
            ReferredUserId = 456
        };
        var expectedResponse = new ReferralResponse
        {
            ReferredUserId = request.ReferredUserId
        };
        var result = new GenericResult<ReferralResponse>(expectedResponse);

        _mockReferralService.Setup(x => x.CreateReferral(request)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.CreateReferral(request);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<ObjectResult>());
        var objectResult = (ObjectResult)actionResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(201));
        Assert.That(objectResult.Value, Is.EqualTo(expectedResponse));
        _mockReferralService.Verify(x => x.CreateReferral(request), Times.Once);
    }

    [Test]
    public async Task CreateReferral_WhenServiceReturnsError_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new CreateReferralRequest
        {
            ReferrerUserId = 123,
            ReferredUserId = 456
        };
        var result = new GenericResult<ReferralResponse>("Referral already exists", ErrorCode.InvalidOperation);

        _mockReferralService.Setup(x => x.CreateReferral(request)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.CreateReferral(request);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)actionResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Referral already exists"));
    }

    [Test]
    public async Task CreateReferral_WhenUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new CreateReferralRequest
        {
            ReferrerUserId = 123,
            ReferredUserId = 456
        };
        var result = new GenericResult<ReferralResponse>("Unauthorized", ErrorCode.Unauthorized);

        _mockReferralService.Setup(x => x.CreateReferral(request)).ReturnsAsync(result);

        // Act
        var actionResult = await _controller.CreateReferral(request);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<UnauthorizedObjectResult>());
    }

    #endregion

    #region CompleteReferral Tests

    [Test]
    public async Task CompleteReferral_WhenValidUserId_ShouldReturnOk()
    {
        // Arrange
        const long referredUserId = 789;

        _mockReferralService.Setup(x => x.CompleteReferral(It.IsAny<long>()))
            .ReturnsAsync(new GenericResult<int>(1));

        // Act
        var actionResult = await _controller.CompleteReferral(referredUserId);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)actionResult;
        Assert.That(okResult.Value, Is.EqualTo(1));
        _mockReferralService.Verify(x => x.CompleteReferral(referredUserId), Times.Once);
    }

    #endregion

    #region GetUserReferralLink Tests

    [Test]
    public async Task GetUserReferralLink_WhenUserIsAuthenticated_ShouldReturnOkWithLink()
    {
        // Arrange
        const long userId = 123;
        const string expectedLink = "https://example.com/referral/abc123";
        var result = new GenericResult<string>(expectedLink);

        _mockUserContext.Setup(x => x.IsAuthenticated).Returns(true);
        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockUserService.Setup(x => x.GetUserReferralLink(userId)).Returns(result);

        // Act
        var actionResult = await _controller.GetUserReferralLink();

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)actionResult;
        Assert.That(okResult.Value, Is.EqualTo(expectedLink));
        _mockUserService.Verify(x => x.GetUserReferralLink(userId), Times.Once);
    }

    [Test]
    public async Task GetUserReferralLink_WhenUserIsNotAuthenticated_ShouldReturnUnauthorized()
    {
        // Arrange
        _mockUserContext.Setup(x => x.IsAuthenticated).Returns(false);

        // Act
        var actionResult = await _controller.GetUserReferralLink();

        // Assert
        Assert.That(actionResult, Is.InstanceOf<UnauthorizedObjectResult>());
        var unauthorizedResult = (UnauthorizedObjectResult)actionResult;
        Assert.That(unauthorizedResult.Value, Is.EqualTo("User is not authenticated"));
        _mockUserService.Verify(x => x.GetUserReferralLink(It.IsAny<long>()), Times.Never);
    }

    [Test]
    public async Task GetUserReferralLink_WhenServiceReturnsError_ShouldReturnBadRequest()
    {
        // Arrange
        const long userId = 123;
        var result = new GenericResult<string>("Error generating link", ErrorCode.GenericError);

        _mockUserContext.Setup(x => x.IsAuthenticated).Returns(true);
        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockUserService.Setup(x => x.GetUserReferralLink(userId)).Returns(result);

        // Act
        var actionResult = await _controller.GetUserReferralLink();

        // Assert
        Assert.That(actionResult, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)actionResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Error generating link"));
    }

    [Test]
    public async Task GetUserReferralLink_WhenServiceReturnsNotFound_ShouldReturnNotFound()
    {
        // Arrange
        const long userId = 999;
        var result = new GenericResult<string>("User not found", ErrorCode.NotFound);

        _mockUserContext.Setup(x => x.IsAuthenticated).Returns(true);
        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockUserService.Setup(x => x.GetUserReferralLink(userId)).Returns(result);

        // Act
        var actionResult = await _controller.GetUserReferralLink();

        // Assert
        Assert.That(actionResult, Is.InstanceOf<NotFoundObjectResult>());
    }

    #endregion
}