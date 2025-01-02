using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.CQRS.Profiles.Queries;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Profiles;
using OMSV1.Infrastructure.Extensions;
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
          // Get all roles
        [HttpGet("all-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _mediator.Send(new GetAllRolesQuery()); // Fetch all roles from the query handler
                if (roles == null || roles.Count == 0)
                {
                    return NotFound(new { message = "No roles found." }); // Return a 404 if no roles are found
                }

                return Ok(roles); // Return the list of roles as the response
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and return a structured error response
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving roles.", new[] { ex.Message });
            }
        }
         // Search Profiles with filters
        [HttpPost("search")]
        //[RequirePermission("DamagedDevice:read")]

        public async Task<IActionResult> GetProfilesWithUsersAndRoles([FromBody] SearchProfilesQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result); // Add pagination headers
                return Ok(result);  // Return the search result
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });  // Return 500 if any error occurs
            }
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
            [HttpPost("search")]
            public async Task<IActionResult> SearchProfiles([FromBody] SearchProfilesQuery query)
            {
                try
                {
                    var result = await _mediator.Send(query);
                    Response.AddPaginationHeader(result);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        message = "An error occurred while processing your request.",
                        details = ex.InnerException?.Message ?? ex.Message
                    });
                }
            }

    }
}
