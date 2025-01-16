using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Offices;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Offices;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Offices
{
    public class OfficeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public OfficeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Office
        [HttpGet]
        public async Task<IActionResult> GetAllOffices([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var offices = await _mediator.Send(new GetAllOfficesQuery(paginationParams));
                Response.AddPaginationHeader(offices);  // Add pagination headers
                return Ok(offices);  // Returns PagedList<OfficeDto>
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the offices.", new[] { ex.Message });
            }
        }

        // GET: api/Office/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetOfficeById(Guid id)
        {
            try
            {
                var office = await _mediator.Send(new GetOfficeByIdQuery(id));
                if (office == null)
                {
                    return NotFound($"Office with ID {id} not found.");
                }
                return Ok(office); // Returns OfficeDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the office by ID.", new[] { ex.Message });
            }
        }

        // POST: api/Office
[HttpPost]
[Authorize(Policy = "RequireAdminRole")]
public async Task<IActionResult> CreateOffice([FromBody] CreateOfficeDto officeDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        // Manually map CreateOfficeDto to AddOfficeCommand
        var command = new AddOfficeCommand
        {
            Name = officeDto.Name,
            Code = officeDto.Code,
            ReceivingStaff = officeDto.ReceivingStaff,
            AccountStaff = officeDto.AccountStaff,
            PrintingStaff = officeDto.PrintingStaff,
            QualityStaff = officeDto.QualityStaff,
            DeliveryStaff = officeDto.DeliveryStaff,
            GovernorateId = officeDto.GovernorateId,
            Budget = officeDto.Budget // Explicitly map Budget
        };

        // Delegate the command to the mediator
        var officeId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOfficeById), new { id = officeId }, officeId);
    }
    catch (Exception ex)
    {
        return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while creating the office.", new[] { ex.Message });
    }
}


        // GET: api/Office/dropdown
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetOfficesForDropdown()
        {
            try
            {
                var offices = await _mediator.Send(new GetOfficesForDropdownQuery());
                return Ok(offices);  // Returns List<OfficeDropdownDto>
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving offices for dropdown.", new[] { ex.Message });
            }
        }

        // PUT: api/Office/{id}
        [HttpPut("{id:Guid}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> UpdateOffice(Guid id, [FromBody] UpdateOfficeCommand command)
        {
            try
            {
                if (id != command.OfficeId)
                {
                    return BadRequest("Mismatched office ID in the URL and body.");
                }

                var isUpdated = await _mediator.Send(command);
                if (!isUpdated)
                {
                    return NotFound($"Office with ID {id} not found.");
                }

                return NoContent();  // 204 No Content
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while updating the office.", new[] { ex.Message });
            }
        }

        // DELETE: api/Office/{id}
        [HttpDelete("{id:Guid}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteOffice(Guid id)
        {
            try
            {
                var isDeleted = await _mediator.Send(new DeleteOfficeCommand(id));
                if (!isDeleted)
                {
                    return NotFound($"Office with ID {id} not found.");
                }

                return NoContent();  // 204 No Content
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while deleting the office.", new[] { ex.Message });
            }
        }
    }
}
