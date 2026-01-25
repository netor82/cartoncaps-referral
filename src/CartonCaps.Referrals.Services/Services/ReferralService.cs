using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Models.Referral;
using CartonCaps.Referrals.Services.Models.Shared;

namespace CartonCaps.Referrals.Services.Services;

public class ReferralService : IReferralService
{
    public GenericResult<bool> CompleteReferral(long referredUserId)
    {
        return new GenericResult<bool>(true);
    }

    public GenericResult<ReferralResponse> CreateReferral(long referredUserId)
    {
        return new GenericResult<ReferralResponse>(new ReferralResponse
        {
            ReferredUserId = referredUserId,
            Status = 1,
            ReferredOn = DateTime.UtcNow,
            CompletedOn = null
        });
    }

    public GenericResult<ReferralListResponse> GetReferralsForUser(long userId)
    {
        return new GenericResult<ReferralListResponse>(new ReferralListResponse
        {
            ReferrerUserId = userId,
            Data = []
        });
    }
}
