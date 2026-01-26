using CartonCaps.Referrals.Data.Enums;

namespace CartonCaps.Referrals.Data.Models;

public class Referral
{
    public long Id { get; set; }
    /// <summary>
    /// User who referred another user.
    /// </summary>
    public long ReferrerUserId { get; set; }
    /// <summary>
    /// User that completed the registration process with a referral code.
    /// </summary>
    public long ReferredUserId { get; set; }
    public ReferralStatus Status { get; set; }
    public DateTime ReferredOn { get; set; }
    public DateTime? CompletedOn { get; set; }
}
