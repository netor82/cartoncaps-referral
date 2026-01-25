using CartonCaps.Referrals.Services.Enums;
using CartonCaps.Referrals.Services.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Referrals.Api.Controllers;

public class BaseController : Controller
{
    protected IActionResult HandleResult<T>(GenericResult<T> result)
    {
        if (result.Success)
        {
            return Ok(result.Data);
        }

        if (result.ErrorCodes.Contains(ErrorCode.Unauthorized))
        {
            return Unauthorized(string.Join('\n', result.Errors));
        }

        if (result.ErrorCodes.Contains(ErrorCode.NotFound))
        {
            return Unauthorized(string.Join('\n', result.Errors));
        }

        return BadRequest(string.Join('\n', result.Errors));
    }
}
