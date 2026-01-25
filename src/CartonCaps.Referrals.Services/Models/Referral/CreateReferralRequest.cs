namespace CartonCaps.Referrals.Services.Models.Referral;

public class CreateReferralRequest
{
    public Guid ReferrerUserId { get; set; }
    public Guid ReferredUserId { get; set; }
}
