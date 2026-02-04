using CartonCaps.Referrals.Data;
using CartonCaps.Referrals.Data.Enums;
using CartonCaps.Referrals.Data.Models;
using CartonCaps.Referrals.Services.Enums;
using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Models.Referral;
using CartonCaps.Referrals.Services.Models.Shared;
using Microsoft.Extensions.Logging;

namespace CartonCaps.Referrals.Services.Services;

public class ReferralService(IGenericRepository<Referral> repository, ILogger<ReferralService> logger) : IReferralService
{
    /// <inheritdoc/>
    public async Task<GenericResult<ReferralResponse>> CreateReferral(CreateReferralRequest request)
    {
        if (request.ReferredUserId == request.ReferrerUserId)
        {
            var result = new GenericResult<ReferralResponse>();
            result.SetError("A user cannot refer themselves", ErrorCode.InvalidOperation);
            return result;
        }

        var existingRecords = await repository.Count(repository.DbSet.Where(r =>
            r.ReferredUserId == request.ReferredUserId));

        if (existingRecords > 0)
        {
            var result = new GenericResult<ReferralResponse>();
            result.SetError("A referral for the referred user already exists.", ErrorCode.InvalidOperation);
            return result;
        }

        var newData = new Referral
        {
            ReferrerUserId = request.ReferrerUserId,
            ReferredUserId = request.ReferredUserId,
            Status = ReferralStatus.Pending,
            ReferredOn = DateTime.UtcNow,
            CompletedOn = null
        };
        await repository.Insert(newData);
        await repository.Save();
        return new GenericResult<ReferralResponse>(ReferralResponse.FromDataModel(newData)!);
    }

    /// <inheritdoc/>
    public async Task<GenericResult<ReferralListResponse>> GetReferralsForUser(long userId)
    {
        var query = repository.DbSet.Where(r => r.ReferrerUserId == userId);
        var data = await repository.ToList(query);
        return new GenericResult<ReferralListResponse>(new ReferralListResponse
        {
            ReferrerUserId = userId,
            Data = data.Select(d => ReferralResponse.FromDataModel(d)!).ToArray()
        });
    }

    /// <inheritdoc/>
    public async Task<GenericResult<int>> CompleteReferral(long referredUserId)
    {
        //update ReferredUserId using EF bulk update
        var rowsUpdated = await repository.BulkUpdate(
            r => r.ReferredUserId == referredUserId && r.Status == ReferralStatus.Pending,
            r =>
            {
                r.SetProperty(obj => obj.Status, ReferralStatus.Completed);
                r.SetProperty(obj => obj.CompletedOn, DateTime.UtcNow);
            });

        if (rowsUpdated > 1)
        {
            logger.LogWarning("More than one referral marked as completed for UserId {id}", referredUserId);
        }
        else if (rowsUpdated == 0)
        {
            logger.LogInformation("No records marked as completed for UserId {id}", referredUserId);
        }

        return new GenericResult<int>(rowsUpdated);
    }
}
