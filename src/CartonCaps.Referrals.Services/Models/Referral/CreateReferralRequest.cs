using System.ComponentModel.DataAnnotations;

namespace CartonCaps.Referrals.Services.Models.Referral;

public class CreateReferralRequest
{
    [Range(1, long.MaxValue)]
    public long ReferrerUserId { get; set; }
    [Range(1, long.MaxValue)]
    public long ReferredUserId { get; set; }
}
