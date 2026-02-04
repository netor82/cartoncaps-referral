using CartonCaps.Referrals.Data;
using CartonCaps.Referrals.Services.Clients;
using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartonCaps.Referrals.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IReferralService, ReferralService>();
        services.AddScoped<IUserClient, UserClient>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDeferredDeepLinkClient, DeferredDeepLinkClient>();

        services.AddDataServices(configuration);

        return services;
    }

    public static void InitializeEnvironment(this IServiceProvider serviceProvider)
    {
        serviceProvider.InitializeDatabase();
    }
}
