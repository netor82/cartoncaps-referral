using CartonCaps.Referrals.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Referrals.Data;

public class ReferralDbContext : DbContext
{
    public ICollection<Referral> Referrals { get; set; } = null!;
}
