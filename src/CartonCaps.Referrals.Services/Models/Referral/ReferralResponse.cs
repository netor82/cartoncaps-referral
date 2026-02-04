namespace CartonCaps.Referrals.Services.Models.Referral;

public class ReferralResponse
{
    public long ReferredUserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime ReferredOn { get; set; }
    public DateTime? CompletedOn { get; set; }

    public static ReferralResponse? FromDataModel(Data.Models.Referral? src) => src == null ? null :
        new ReferralResponse
        {
            ReferredUserId = src.ReferredUserId,
            Status = (int)src.Status,
            StatusName = src.Status.ToString(),
            ReferredOn = src.ReferredOn,
            CompletedOn = src.CompletedOn
        };
}
