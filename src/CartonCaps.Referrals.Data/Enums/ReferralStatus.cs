namespace CartonCaps.Referrals.Data.Enums;

public enum ReferralStatus
{
    /// <summary>
    /// A user has used the referral but the referred user has not yet completed by
    /// registering the first grocery receipt.
    /// </summary>
    Pending = 0,
    /// <summary>
    /// The referred user has completed the referral by registering their first grocery receipt.
    /// </summary>
    Completed = 1
}
