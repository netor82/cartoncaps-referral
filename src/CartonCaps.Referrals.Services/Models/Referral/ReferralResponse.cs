namespace CartonCaps.Referrals.Services.Models.Referral;

public class ReferralResponse
{
    public Guid Id { get; set; }
    public Guid ReferrerUserId { get; set; }
    public Guid ReferredUserId { get; set; }
    public int Status { get; set; }
    public DateTime ReferredOn { get; set; }
    public DateTime? CompletedOn { get; set; }

    public static ReferralResponse? FromDataModel(Data.Models.Referral? src) => src == null ? null : 
        new ReferralResponse
        {
            Id = src.Id,
            ReferrerUserId = src.ReferrerUserId,
            ReferredUserId = src.ReferredUserId,
            Status = (int)src.Status,
            ReferredOn = src.ReferredOn,
            CompletedOn = src.CompletedOn
        };
}
