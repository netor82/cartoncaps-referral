using CartonCaps.Referrals.Services.Interfaces;

namespace CartonCaps.Referrals.Api.Middlewares;

public class UserContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        if (context.Request.Headers.ContainsKey("X-User-Id"))
        {
            if (long.TryParse(context.Request.Headers["X-User-Id"], out var userId))
            {
                userContext.UserId = userId;
            }
        }
        await next(context);
    }
}
