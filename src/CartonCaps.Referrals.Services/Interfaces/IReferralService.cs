using CartonCaps.Referrals.Services.Models.Referral;
using CartonCaps.Referrals.Services.Models.Shared;

namespace CartonCaps.Referrals.Services.Interfaces;

public interface IReferralService
{
    /// <summary>
    /// Get referrals where the specified user is the referrer.
    /// </summary>
    /// <param name="userId">Referrer user id.</param>
    /// <returns></returns>
    Task<GenericResult<ReferralListResponse>> GetReferralsForUser(long userId);

    /// <summary>
    /// Creates a new referral record.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The created record</returns>
    Task<GenericResult<ReferralResponse>> CreateReferral(CreateReferralRequest request);

    /// <summary>
    /// Marks the referrals as complete for the referred user.
    /// </summary>
    /// <param name="referredUserId">Given referred user id.</param>
    /// <returns>Number of records updated.</returns>
    Task<GenericResult<int>> CompleteReferral(long referredUserId);
}
