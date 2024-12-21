using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Application.Dtos; // Import DTOs namespace
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Infrastructure.Extensions;
using OMSV1.Application.Helpers; // Assuming the DeleteDamagedPassportCommand exists here.
using OMSV1.Application.CQRS.DamagedDevices.Queries;
namespace OMSV1.Application.Controllers.DamagedPassports
{
 
    public class DamagedPassportController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DamagedPassportController(IMediator mediator)
        {
            _mediator = mediator;
        }

       // Get All Damaged Devices with Pagination
       [HttpGet]
        public async Task<IActionResult> GetAllDamagedPassport([FromQuery] PaginationParams paginationParams)
        {
            // Send the pagination parameters to the query handler
            var damagedDevices = await _mediator.Send(new GetAllDamagedPassportsQuery(paginationParams));

            // Add pagination headers to the response
            Response.AddPaginationHeader(damagedDevices);

            // Return the paginated result
            return Ok(damagedDevices);  // Returns PagedList<DamagedDeviceDto>
        }

        // GET method to retrieve a damaged passport by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDamagedPassportById(int id)
        {
            var passport = await _mediator.Send(new GetDamagedPassportByIdQuery(id));
            if (passport == null) return NotFound();
            return Ok(passport);  // This returns a single DamagedPassportDto
        }

        // POST method to add a new damaged passport
        [HttpPost]
        public async Task<IActionResult> AddDamagedPassport([FromBody] AddDamagedPassportCommand command)
        {
            try
            {
                // Use MediatR to handle the logic
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetDamagedPassportById), new { id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT method to update the damaged passport
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDamagedPassport(int id, [FromBody] UpdateDamagedPassportCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"DamagedPassport with ID {id} not found.");
            }

            return NoContent(); // 204 No Content, as no data is returned after a successful update
        }

        // DELETE method to delete the damaged passport
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDamagedPassport(int id)
        {
            var result = await _mediator.Send(new DeleteDamagedPassportCommand(id));

            if (!result)
            {
                return NotFound($"DamagedPassport with ID {id} not found.");
            }

            return NoContent(); // 204 No Content, as the passport has been deleted successfully
        }
                [HttpPost("search")]
        public async Task<IActionResult> GetDamagedDevices([FromBody] GetDamagedPassportQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the error here (if necessary)
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

    }
}
