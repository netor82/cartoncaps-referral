using Asp.Versioning;
using CartonCaps.Referrals.Services.Enums;
using CartonCaps.Referrals.Services.Interfaces;
using CartonCaps.Referrals.Services.Models.Referral;
using CartonCaps.Referrals.Services.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CartonCaps.Referrals.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ReferralsController(IUserContext userContext, IReferralService referralService) : BaseController
{

    /// <summary>
    /// Get referrals for a given user. Intended for communication between services.
    /// </summary>
    /// <param name="userId">Id of the User.</param>
    /// <returns>List of referrals.</returns>
    /// <response code="200">List of referrals filtered by user.</response>
    /// <response code="401">User is not authenticated.</response>
    [HttpGet()]
    [EndpointSummary("Get referrals for a given user. Intended for communication between services.")]
    [ProducesResponseType(typeof(ReferralListResponse), StatusCodes.Status200OK, "application/json", Description = "List of referrals filtered by user.")]
    [ProducesResponseType(typeof(String), StatusCodes.Status401Unauthorized)]
    public IActionResult GetForCurrentUser()
    {
        GenericResult<ReferralListResponse> result;

        if (!userContext.IsAuthenticated)
        {
            result = new GenericResult<ReferralListResponse>("User is not authenticated", ErrorCode.Unauthorized);
        }
        else
        {
            result = referralService.GetReferralsForUser(userContext.UserId);
        }
        return HandleResult(result);
    }


    /// <summary>
    /// Get referrals for a given user.
    /// Intended for communication between services.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>List of referrals.</returns>
    [HttpGet("user/{userId:long}")]
    [EndpointSummary("Get referrals for a given user. Intended for communication between services.")]
    [ProducesResponseType(typeof(ReferralListResponse), StatusCodes.Status200OK, "application/json")]
    public IActionResult GetByUserId([Description("Id of the user.")] long userId)
    {
        GenericResult<ReferralListResponse> result;
        result = referralService.GetReferralsForUser(userId);
        return HandleResult(result);
    }



}

