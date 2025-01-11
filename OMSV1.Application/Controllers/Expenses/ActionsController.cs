using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Authorization.Attributes;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;

namespace OMSV1.Application.Controllers
{
    public class ActionsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ActionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [RequirePermission("EXr")]
        public async Task<IActionResult> GetAllActions()
        {
            try
            {
                var query = new GetAllActionsQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [RequirePermission("EXc")]
        public async Task<IActionResult> AddAction([FromBody] AddActionCommand command)
        {
            try
            {
                if (command == null)
                    return BadRequest("Command cannot be null.");

                var result = await _mediator.Send(command);
                return Ok(new { ActionId = result });
             }
             catch (ArgumentException ex)
             {
                return BadRequest(ex.Message);
             }
             catch (HandlerException ex)
             {
                return StatusCode(500, ex.Message);
             }
             catch (Exception ex)
             {
                return StatusCode(500, $"Internal server error: {ex.Message}");
             }
        }

        
    }
}
