namespace CartonCaps.Referrals.Services.Models.Referral;

public class ReferralListResponse
{
    public long ReferrerUserId { get; set; }
    public ReferralResponse[] Data { get; set; } = [];
}
