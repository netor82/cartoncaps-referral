using CartonCaps.Referrals.Services.Models.Referral;
using CartonCaps.Referrals.Services.Models.Shared;

namespace CartonCaps.Referrals.Services.Interfaces;

public interface IReferralService
{
    GenericResult<ReferralListResponse> GetReferralsForUser(long userId);
    GenericResult<ReferralResponse> CreateReferral(long referredUserId);
    GenericResult<bool> CompleteReferral(long referredUserId);
}
