using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Referrals.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ReferralsController : Controller
{
    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        return Ok();
    }

}

