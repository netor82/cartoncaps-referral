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
public class ReferralsController(
    IUserContext userContext,
    IReferralService referralService,
    IUserService userService) : BaseController
{

    /// <summary>
    /// Gets referrals for a given user. Intended for communication between services.
    /// </summary>
    /// <param name="userId">Id of the User.</param>
    /// <returns>List of referrals.</returns>
    /// <response code="200">List of referrals filtered by user.</response>
    /// <response code="401">User is not authenticated.</response>
    [HttpGet()]
    [EndpointSummary("Gets referrals for a given user. Intended for communication between services.")]
    [ProducesResponseType(typeof(ReferralListResponse), StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(String), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetForCurrentUser()
    {
        GenericResult<ReferralListResponse> result;

        if (!userContext.IsAuthenticated)
        {
            result = new GenericResult<ReferralListResponse>("User is not authenticated", ErrorCode.Unauthorized);
        }
        else
        {
            result = await referralService.GetReferralsForUser(userContext.UserId);
        }
        return HandleResult(result);
    }


    /// <summary>
    /// Gets referrals for a given user.
    /// Intended for communication between services.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>List of referrals.</returns>
    [HttpGet("user/{userId:long}")]
    [EndpointSummary("Gets referrals for a given user. Intended for communication between services.")]
    [ProducesResponseType(typeof(ReferralListResponse), StatusCodes.Status200OK, "application/json")]
    public async Task<IActionResult> GetByUserId([Description("Id of the referrer user.")] long userId)
    {
        var result = await referralService.GetReferralsForUser(userId);
        return HandleResult(result);
    }

    /// <summary>
    /// Creates a new entry. Intended for communication between services.
    /// </summary>
    /// <param name="request">Information of referrer and referred users.</param>
    /// <returns></returns>
    [HttpPost]
    [EndpointSummary("Creates a new entry. Intended for communication between services.")]
    [ProducesResponseType(typeof(ReferralResponse), StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> CreateReferral([FromBody]CreateReferralRequest request)
    {
        var result = await referralService.CreateReferral(request);
        return HandleResult(result, isCreate: true);
    }

    /// <summary>
    /// Marks referrals associated with the userId as Completed.
    /// </summary>
    /// <param name="referredUserId">Id of the referred user.</param>
    /// <returns></returns>
    [HttpPatch("user/{referredUserId:long}")]
    [EndpointSummary("Marks referrals associated with the userId as Completed.")]
    public async Task<IActionResult> CompleteReferral([Description("Id of the referred user.")] long referredUserId)
    {
        var result = await referralService.CompleteReferral(referredUserId);
        return HandleResult(result);
    }

    [HttpGet("referral-link")]
    [EndpointSummary("Gets or creates a referral link for user id.")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserReferralLink()
    {
        GenericResult<string> result;

        if (!userContext.IsAuthenticated)
        {
            result = new GenericResult<string>("User is not authenticated", ErrorCode.Unauthorized);
        }
        else
        {
            result = userService.GetUserReferralLink(userContext.UserId);
        }
        return HandleResult(result);
    }
}

