namespace CartonCaps.Referrals.Services.Models.Referral;

public class ReferralResponse
{
    public long ReferredUserId { get; set; }
    public int Status { get; set; }
    public DateTime ReferredOn { get; set; }
    public DateTime? CompletedOn { get; set; }

    public static ReferralResponse? FromDataModel(Data.Models.Referral? src) => src == null ? null :
        new ReferralResponse
        {
            ReferredUserId = src.ReferredUserId,
            Status = (int)src.Status,
            ReferredOn = src.ReferredOn,
            CompletedOn = src.CompletedOn
        };
}
