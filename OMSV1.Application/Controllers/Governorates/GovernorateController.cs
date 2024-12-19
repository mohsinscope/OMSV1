using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Infrastructure.Extensions;

namespace OMSV1.Application.Controllers.Governorates
{
public class GovernorateController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

// GET: api/Governorate
[HttpGet]
public async Task<IActionResult> GetAllGovernorates([FromQuery] PaginationParams paginationParams)
{
    var governorates = await _mediator.Send(new GetAllGovernoratesQuery(paginationParams));
    Response.AddPaginationHeader(governorates);
    return Ok(governorates); // Returns PagedList<GovernorateDto>
}

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGovernorateById(int id)
    {
        var governorate = await _mediator.Send(new GetGovernorateByIdQuery(id));
        if (governorate == null)
            return NotFound($"Governorate with ID {id} not found.");

        return Ok(governorate); // Returns GovernorateDto
    }

    [HttpPost]
    public async Task<IActionResult> CreateGovernorate([FromBody] CreateGovernorateDto governorateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new AddGovernorateCommand(governorateDto.Name, governorateDto.Code);
        var governorateId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetGovernorateById), new { id = governorateId }, governorateId);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGovernorate(int id, [FromBody] UpdateGovernorateCommand command)
    {
        if (id != command.Id)
            return BadRequest("Mismatched governorate ID in the URL and body.");

        var isUpdated = await _mediator.Send(command);
        if (!isUpdated)
            return NotFound($"Governorate with ID {id} not found.");

        return NoContent();
    }
    [HttpGet("{id:int}/with-offices")]
public async Task<IActionResult> GetGovernorateWithOffices(int id)
{
    var governorate = await _mediator.Send(new GetGovernorateWithOfficesQuery(id));
    if (governorate == null)
        return NotFound($"Governorate with ID {id} not found.");

    return Ok(governorate);
}


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGovernorate(int id)
    {
        var isDeleted = await _mediator.Send(new DeleteGovernorateCommand(id));
        if (!isDeleted)
            return NotFound($"Governorate with ID {id} not found.");

        return NoContent();
    }
}

}
