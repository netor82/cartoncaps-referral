using CartonCaps.Referrals.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Referrals.Data;

public class ReferralDbContext(DbContextOptions<ReferralDbContext> options) : DbContext(options)
{
    public DbSet<Referral> Referrals { get; set; } = null!;
}
