using CartonCaps.Referrals.Services.Models.Shared;
using CartonCaps.Referrals.Services.Models.User;

namespace CartonCaps.Referrals.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Retrieves the user associated with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve. Must be a positive value.</param>
    /// <returns>A <see cref="GenericResult{User}"/> containing the user if found; otherwise, a result indicating failure.</returns>
    GenericResult<User> GetUser(long id);

    /// <summary>
    /// Gets or creates a referral link for given user id.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>A <see cref="GenericResult{string}"/> containing the referral link if found; otherwise, a result indicating failure.</returns>
    GenericResult<string> GetUserReferralLink(long userId);
}
