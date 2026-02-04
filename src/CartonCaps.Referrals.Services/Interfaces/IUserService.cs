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
    /// Retrieves the users associated with the specified unique identifiers.
    /// </summary>
    /// <param name="userIds">An array of unique identifiers for the users to retrieve. Must not be empty.</param>
    /// <returns>A <see cref="GenericResult{List{User}}"/> containing the users if found; otherwise, a result indicating failure.</returns>
    GenericResult<List<User>> GetUsersByIds(long[] userIds);


    /// <summary>
    /// Gets or creates a referral link for given user id.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>A <see cref="GenericResult{string}"/> containing the referral link if found; otherwise, a result indicating failure.</returns>
    GenericResult<string> GetUserReferralLink(long userId);
}
