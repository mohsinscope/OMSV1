using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Infrastructure.Extensions;
using System.Net;
using OMSV1.Application.CQRS.Governorates;
using Microsoft.AspNetCore.Authorization;

namespace OMSV1.Application.Controllers.Governorates
{
    public class GovernorateController : BaseApiController
    {
        private readonly IMediator _mediator;

        public GovernorateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Governorate
        [HttpGet]
        public async Task<IActionResult> GetAllGovernorates([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var governorates = await _mediator.Send(new GetAllGovernoratesQuery(paginationParams));
                Response.AddPaginationHeader(governorates);
                return Ok(governorates); // Returns PagedList<GovernorateDto>
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the governorates.", new[] { ex.Message });
            }
        }
        

        // GET: api/Governorate/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetGovernorateById(Guid id)
        {
            try
            {
                var governorate = await _mediator.Send(new GetGovernorateByIdQuery(id));
                if (governorate == null)
                    return NotFound($"Governorate with ID {id} not found.");

                return Ok(governorate); // Returns GovernorateDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the governorate by ID.", new[] { ex.Message });
            }
        }

        // GET: api/Governorate/dropdown
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetGovernoratesForDropdown()
        {
            try
            {
                var governorates = await _mediator.Send(new GetGovernoratesForDropdownQuery());
                return Ok(governorates); // Returns List<GovernDropdownDto>
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving governorates for dropdown.", new[] { ex.Message });
            }
        }

        // GET: api/Governorate/Dropdown/{governorateId}
        [HttpGet("Dropdown/{governorateId}")]
        public async Task<IActionResult> GetOfficesByGovernorate(Guid governorateId)
        {
            try
            {
                var query = new GetGovernoratesWithOfficesForDropdownQuery(governorateId);
                var offices = await _mediator.Send(query);

                if (offices == null || !offices.Any())
                {
                    return NotFound("No offices found for the provided governorate.");
                }

                return Ok(offices);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving offices for the governorate.", new[] { ex.Message });
            }
        }

        // POST: api/Governorate
        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateGovernorate([FromBody] CreateGovernorateDto governorateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = new AddGovernorateCommand(governorateDto.Name, governorateDto.Code);
                var governorateId = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetGovernorateById), new { id = governorateId }, governorateId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Governorate/{id}
        [HttpPut("{id:Guid}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> UpdateGovernorate(Guid id, [FromBody] UpdateGovernorateCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match the ID in the request body.");
            }

            var isUpdated = await _mediator.Send(command);
            if (!isUpdated)
            {
                return NotFound($"Governorate with ID {id} not found.");
            }

            return NoContent(); // 204 No Content
        }

        // GET: api/Governorate/{id}/with-offices
        [HttpGet("{id:Guid}/with-offices")]
        public async Task<IActionResult> GetGovernorateWithOffices(Guid id)
        {
            try
            {
                var governorate = await _mediator.Send(new GetGovernorateWithOfficesQuery(id));
                if (governorate == null)
                    return NotFound($"Governorate with ID {id} not found.");

                return Ok(governorate);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving governorate with offices.", new[] { ex.Message });
            }
        }
        [HttpPost("search-offices")]
        public async Task<IActionResult> SearchOffices([FromBody] SearchOfficesQuery request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result); // Return the result (List<OfficeCountDto>) based on the query
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and return a structured error response
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while searching for offices.", new[] { ex.Message });
            }
        }


        // DELETE: api/Governorate/{id}
        [HttpDelete("{id:Guid}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteGovernorate(Guid id)
        {
            try
            {
                var isDeleted = await _mediator.Send(new DeleteGovernorateCommand(id));
                if (!isDeleted)
                    return NotFound($"Governorate with ID {id} not found.");

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
