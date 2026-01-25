using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CartonCaps.Referrals.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IReferralService, ReferralService>();
        return services;
    }
}
