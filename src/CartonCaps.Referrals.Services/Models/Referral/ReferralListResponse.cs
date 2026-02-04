namespace CartonCaps.Referrals.Services.Models.Referral;

public class ReferralListResponse
{
    public long ReferrerUserId { get; set; }
    public string ReferralCode { get; set; } = string.Empty;
    public ReferralResponse[] Data { get; set; } = [];
}
