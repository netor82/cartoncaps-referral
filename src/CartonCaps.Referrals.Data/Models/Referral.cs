using CartonCaps.Referrals.Data.Enums;

namespace CartonCaps.Referrals.Data.Models;

public class Referral
{
    public long Id { get; set; }
    public long ReferrerUserId { get; set; }
    public long ReferredUserId { get; set; }
    public ReferralStatus Status { get; set; }
    public DateTime ReferredOn { get; set; }
    public DateTime? CompletedOn { get; set; }
}
