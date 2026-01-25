namespace CartonCaps.Referrals.Api;

public static class DependencyInjection
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    /// <param name="services">Service collections to be modified</param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {

        // api versioning
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        ;

        services.AddControllers();

        // Swagger support
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    /// <summary>
    /// Configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseWebServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();
        return app;
    }
}
