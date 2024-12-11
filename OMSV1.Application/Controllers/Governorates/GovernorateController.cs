using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Queries.Governorates;

namespace OMSV1.Application.Controllers.Governorates
{
    [ApiController]
[Route("api/[controller]")]
public class GovernorateController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllGovernorates()
    {
        var governorates = await _mediator.Send(new GetAllGovernoratesQuery());
        return Ok(governorates); // Returns List<GovernorateDto>
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
