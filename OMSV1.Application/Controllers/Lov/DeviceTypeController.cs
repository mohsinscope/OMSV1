using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.LOV;
using OMSV1.Application.CQRS.Lov.DamagedDevice;
using OMSV1.Application.Helpers;
using System.Net;

namespace OMSV1.Application.Controllers.LOV
{
    public class DeviceTypeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DeviceTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Add DeviceType
        [HttpPost]
        public async Task<IActionResult> AddDeviceType([FromBody] AddDeviceTypeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result)
                {
                    return Ok("Device Type added successfully");
                }
                return BadRequest("Failed to add Device Type");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while adding the device type.", new[] { ex.Message });
            }
        }

        // Get All DeviceTypes
        [HttpGet]
        public async Task<IActionResult> GetAllDeviceTypes()
        {
            try
            {
                var query = new GetAllDeviceTypesQuery(); 
                var deviceTypes = await _mediator.Send(query);  
                return Ok(deviceTypes);  
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving device types.", new[] { ex.Message });
            }
        }

        // Get DeviceType by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceTypeById(int id)
        {
            try
            {
                var query = new GetDeviceTypesQueryById(id); 
                var deviceType = await _mediator.Send(query); 

                if (deviceType == null)
                {
                    return NotFound($"Device Type with ID {id} not found.");
                }

                return Ok(deviceType); 
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the device type by ID.", new[] { ex.Message });
            }
        }

        // Update DeviceType
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeviceType(int id, [FromBody] UpdateDeviceTypeCommand command)
        {
            try
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
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while updating the device type.", new[] { ex.Message });
            }
        }

        // Delete DeviceType
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceType(int id)
        {
            try
            {
                var command = new DeleteDeviceTypeCommand(id); 
                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok("Device Type deleted successfully");
                }

                return BadRequest("Failed to delete Device Type");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while deleting the device type.", new[] { ex.Message });
            }
        }
    }
}
