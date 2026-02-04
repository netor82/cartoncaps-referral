using CartonCaps.Referrals.Data;
using CartonCaps.Referrals.Data.Enums;
using CartonCaps.Referrals.Data.Models;
using CartonCaps.Referrals.Services.Enums;
using CartonCaps.Referrals.Services.Models.Referral;
using CartonCaps.Referrals.Services.Services;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CartonCaps.Referrals.UnitTests;

[TestFixture]
public class ReferralServiceTests
{
    private Mock<IGenericRepository<Referral>> _mockRepository;
    private Mock<ILogger<ReferralService>> _mockLogger;
    private ReferralService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IGenericRepository<Referral>>();
        _mockLogger = new Mock<ILogger<ReferralService>>();
        _service = new ReferralService(_mockRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task CreateReferral_WhenNoExistingReferral_ShouldCreateSuccessfully()
    {
        // Arrange
        var request = new CreateReferralRequest
        {
            ReferrerUserId = 1,
            ReferredUserId = 20
        };

        var mockDbSet = new List<Referral>().AsQueryable();
        _mockRepository.Setup(r => r.DbSet).Returns(mockDbSet);
        _mockRepository.Setup(r => r.Count(It.IsAny<IQueryable<Referral>>())).ReturnsAsync(0);
        _mockRepository.Setup(r => r.Insert(It.IsAny<Referral>())).Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.Save()).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateReferral(request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data?.ReferredUserId, Is.EqualTo(request.ReferredUserId));
            Assert.That(result.Data?.Status, Is.EqualTo((int)ReferralStatus.Pending));
            Assert.That(result.Data?.CompletedOn, Is.Null);
        }

        _mockRepository.Verify(r => r.Insert(It.Is<Referral>(
            refe => refe.ReferrerUserId == request.ReferrerUserId &&
                    refe.ReferredUserId == request.ReferredUserId &&
                    refe.Status == ReferralStatus.Pending)), Times.Once);
        _mockRepository.Verify(r => r.Save(), Times.Once);
    }

    [Test]
    public async Task CreateReferral_WhenReferralAlreadyExists_ShouldReturnError()
    {
        // Arrange
        var request = new CreateReferralRequest
        {
            ReferrerUserId = 1,
            ReferredUserId = 2
        };

        var existingReferrals = new List<Referral>
        {
            new()
            {
                Id = 1,
                ReferrerUserId = 1,
                ReferredUserId = 2,
                Status = ReferralStatus.Pending
            }
        }.AsQueryable();

        _mockRepository.Setup(r => r.DbSet).Returns(existingReferrals);
        _mockRepository.Setup(r => r.Count(It.IsAny<IQueryable<Referral>>())).ReturnsAsync(1);

        // Act
        var result = await _service.CreateReferral(request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Errors, Contains.Item("A referral for the referred user already exists."));
            Assert.That(result.ErrorCodes, Contains.Item(ErrorCode.InvalidOperation));
        }

        _mockRepository.Verify(r => r.Insert(It.IsAny<Referral>()), Times.Never);
        _mockRepository.Verify(r => r.Save(), Times.Never);
    }

    [Test]
    public async Task GetReferralsForUser_WhenReferralsExist_ShouldReturnReferrals()
    {
        // Arrange
        long userId = 1;
        var referrals = new List<Referral>
        {
            new()
            {
                Id = 1654,
                ReferrerUserId = userId,
                ReferredUserId = 2,
                Status = ReferralStatus.Pending,
                ReferredOn = DateTime.UtcNow.AddDays(-5)
            },
            new()
            {
                Id = 2984,
                ReferrerUserId = userId,
                ReferredUserId = 3,
                Status = ReferralStatus.Completed,
                ReferredOn = DateTime.UtcNow.AddDays(-10),
                CompletedOn = DateTime.UtcNow.AddDays(-3)
            }
        };

        var mockDbSet = referrals.AsQueryable();
        _mockRepository.Setup(r => r.DbSet).Returns(mockDbSet);
        _mockRepository.Setup(r => r.ToList(It.IsAny<IQueryable<Referral>>())).ReturnsAsync(referrals);

        // Act
        var result = await _service.GetReferralsForUser(userId);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data?.ReferrerUserId, Is.EqualTo(userId));
            Assert.That(result.Data?.Data, Has.Length.EqualTo(2));
            Assert.That(result.Data?.Data[0].ReferredUserId, Is.EqualTo(2));
            Assert.That(result.Data?.Data[1].ReferredUserId, Is.EqualTo(3));
        }
    }

    [Test]
    public async Task GetReferralsForUser_WhenNoReferralsExist_ShouldReturnEmptyList()
    {
        // Arrange
        long userId = 1;
        var emptyReferrals = new List<Referral>();

        var mockDbSet = emptyReferrals.AsQueryable();
        _mockRepository.Setup(r => r.DbSet).Returns(mockDbSet);
        _mockRepository.Setup(r => r.ToList(It.IsAny<IQueryable<Referral>>())).ReturnsAsync(emptyReferrals);

        // Act
        var result = await _service.GetReferralsForUser(userId);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data?.ReferrerUserId, Is.EqualTo(userId));
            Assert.That(result.Data?.Data, Is.Empty);
        }
    }

    [Test]
    public async Task CompleteReferral_WhenOneReferralUpdated_ShouldReturnSuccessWithNoWarning()
    {
        // Arrange
        long referredUserId = 2;
        _mockRepository.Setup(r => r.BulkUpdate(
            It.IsAny<Expression<Func<Referral, bool>>>(),
            It.IsAny<Action<UpdateSettersBuilder<Referral>>>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.CompleteReferral(referredUserId);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.EqualTo(1));
        }

        _mockRepository.Verify(r => r.BulkUpdate(
            It.IsAny<Expression<Func<Referral, bool>>>(),
            It.IsAny<Action<UpdateSettersBuilder<Referral>>>()), Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Test]
    public async Task CompleteReferral_WhenNoReferralUpdated_ShouldReturnZeroAndLogInformation()
    {
        // Arrange
        long referredUserId = 2;
        _mockRepository.Setup(r => r.BulkUpdate(
            It.IsAny<Expression<Func<Referral, bool>>>(),
            It.IsAny<Action<UpdateSettersBuilder<Referral>>>()))
            .ReturnsAsync(0);

        // Act
        var result = await _service.CompleteReferral(referredUserId);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.Zero);
        }

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No records marked as completed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task CompleteReferral_WhenMultipleReferralsUpdated_ShouldReturnCountAndLogWarning()
    {
        // Arrange
        long referredUserId = 2;
        _mockRepository.Setup(r => r.BulkUpdate(
            It.IsAny<Expression<Func<Referral, bool>>>(),
            It.IsAny<Action<UpdateSettersBuilder<Referral>>>()))
            .ReturnsAsync(2);

        // Act
        var result = await _service.CompleteReferral(referredUserId);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.EqualTo(2));
        }

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("More than one referral marked as completed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}