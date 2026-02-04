namespace CartonCaps.Referrals.Services.Models.User;

public class User
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string ReferralCode { get; set; } = null!;
}
