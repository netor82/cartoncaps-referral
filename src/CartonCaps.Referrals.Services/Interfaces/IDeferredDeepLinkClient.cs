namespace CartonCaps.Referrals.Services.Interfaces
{
    /// <summary>
    /// Defines methods to interact with Deferred Deep Link Service API.
    /// </summary>
    public interface IDeferredDeepLinkClient
    {
        /// <summary>
        /// Get link to be used for deferred deep linking
        /// </summary>
        /// <param name="referralCode">User referral code</param>
        /// <returns>Link to be used in emails, messages, SMS and more.</returns>
        string GetDeferredLink(string referralCode);
    }
}
