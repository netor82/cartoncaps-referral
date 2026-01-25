using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartonCaps.Referrals.Data;

public static class DependencyInjection
{
    public static void AddDataServices(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddDbContext<ReferralDbContext>(options => options.UseSqlite(ConnectionString(configuration)));
    }

    public static string ConnectionString(IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Referrals");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ApplicationException("Database not configured");
        }
        return connectionString;
    }

    /// <summary>
    /// Executes the Update method of EF
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReferralDbContext>();
        dbContext.Database.Migrate();
    }
}
