using CartonCaps.Referrals.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Referrals.Data.Models;

[Index(nameof(ReferrerUserId))]
[Index(nameof(ReferredUserId), IsUnique = true)]
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
