namespace CartonCaps.Referrals.Services.Interfaces;

public interface IUserContext
{
    long UserId { get; set; }
    bool IsAuthenticated { get; }
}
