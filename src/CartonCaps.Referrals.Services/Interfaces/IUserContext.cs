namespace CartonCaps.Referrals.Services.Interfaces;

/// <summary>
/// Represents information about the user of a given authenticated request
/// </summary>
public interface IUserContext
{
    long UserId { get; set; }
    bool IsAuthenticated { get; }
}
