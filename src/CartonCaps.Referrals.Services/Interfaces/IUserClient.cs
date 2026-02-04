using CartonCaps.Referrals.Services.Models.User;

namespace CartonCaps.Referrals.Services.Interfaces;

/// <summary>
/// Defines methods for retrieving user information from a User Service API.
/// </summary>
public interface IUserClient
{
    User GetUser(long id);
}
