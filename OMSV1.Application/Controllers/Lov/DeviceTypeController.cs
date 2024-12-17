using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.LOV;
using OMSV1.Application.CQRS.Lov.DamagedDevice;
using OMSV1.Application.Dtos.LOV;
using System.Threading.Tasks;

namespace OMSV1.Application.Controllers.LOV
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeviceTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Add DeviceType
        [HttpPost]
        public async Task<ActionResult<DeviceTypeDto>> AddDeviceType([FromBody] AddDeviceTypeCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok("Device Type added successfully");
            }
            return BadRequest("Failed to add Device Type");
        }

        // Get All DeviceTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceTypeDto>>> GetAllDeviceTypes()
        {
            var query = new GetAllDeviceTypesQuery(); // Assuming you have a query handler for fetching device types
            var deviceTypes = await _mediator.Send(query);  // Send the query using MediatR

            return Ok(deviceTypes);  // Return the fetched list
        }

        // Get DeviceType by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceTypeDto>> GetDeviceTypeById(int id)
        {
            var query = new GetDeviceTypesQueryById(id); // Create a query to get a device type by its Id
            var deviceType = await _mediator.Send(query); // Send the query

            if (deviceType == null)
                return NotFound($"Device Type with ID {id} not found.");

            return Ok(deviceType); // Return the device type
        }

        // Update DeviceType
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDeviceType(int id, [FromBody] UpdateDeviceTypeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Device Type ID mismatch.");
            }

            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok("Device Type updated successfully");
            }

            return BadRequest("Failed to update Device Type");
        }

        // Delete DeviceType
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeviceType(int id)
        {
            // Pass the id to the DeleteDeviceTypeCommand
            var command = new DeleteDeviceTypeCommand(id);  // Pass the id to the command constructor
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok("Device Type deleted successfully");
            }

            return BadRequest("Failed to delete Device Type");
        }
    }
}
