using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Profiles;

namespace OMSV1.Application.Controllers.Profiles
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }
 // Get Profile ID by UserId (Returns only ID)
    // [HttpGet("user-profile-id")]
    // public async Task<IActionResult> GetUserProfileId()
    // {
    //     var userId = User.GetUserId(); // Assume this gets the authenticated user's ID
    //     var profileId = await _mediator.Send(new GetProfileIdByUserIdQuery(userId));
    //     return Ok(profileId);
    // }
    
        [HttpGet("user-profile")]
        public async Task<IActionResult> GetProfileByUserId()
        {
            // Extract user ID from the token using ClaimsPrincipalExtensions
            var userId = User.GetUserId();

            // Send the query to Mediator
            var profile = await _mediator.Send(new GetProfileByUserIdQuery(userId));

            return Ok(profile);
        }
    }
}
