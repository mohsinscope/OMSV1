using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Profiles;

namespace OMSV1.Application.Controllers.Profiles
{
public class ProfileController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }
// Get Profile ID by UserId (Returns only ID)

//      [HttpGet("user-profile-id")]

//      public async Task<IActionResult> GetUserProfileId()

//      {

//         var userId = User.GetUserId(); // Assume this gets the authenticated user's ID

//         var profileId = await _mediator.Send(new GetProfileIdByUserIdQuery(userId));

//         return Ok(profileId);

//      }

    [HttpGet("user-profile")]
    public async Task<IActionResult> GetProfileByUserId()
    {
        var userId = User.GetUserId();
        var profile = await _mediator.Send(new GetProfileByUserIdQuery(userId));

        if (profile == null)
        {
            return NotFound(new { message = $"Profile not found for UserId {userId}" });
        }

        return Ok(profile);
    }
}

}
