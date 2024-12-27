using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Profiles;
using System;
using System.Threading.Tasks;
using System.Net;

namespace OMSV1.Application.Controllers.Profiles
{
    public class ProfileController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get Profile by UserId
        [HttpGet("user-profile")]
        public async Task<IActionResult> GetProfileByUserId()
        {
            try
            {
                var userId = User.GetUserId(); // Retrieve the authenticated user's ID
                var profile = await _mediator.Send(new GetProfileByUserIdQuery(userId));

                if (profile == null)
                {
                    return NotFound(new { message = $"Profile not found for UserId {userId}" });
                }

                return Ok(profile); // Return the profile if found
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and return a structured error response
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the profile.", new[] { ex.Message });
            }
        }
    }
}
