using CartonCaps.Referrals.Services.Interfaces;

namespace CartonCaps.Referrals.Services.Clients;

internal class DeferredDeepLinkClient : IDeferredDeepLinkClient
{
    /// <inheritdoc/>
    public string GetDeferredLink(string referralCode)
    {
        return $"https://cartoncaps.link/abfilefa90p?referral_code={referralCode}";
    }
}
