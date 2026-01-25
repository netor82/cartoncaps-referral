using CartonCaps.Referrals.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();
app.UseWebServices();
app.Run();
