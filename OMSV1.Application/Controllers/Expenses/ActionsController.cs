using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Authorization.Attributes;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Actions;
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

        [HttpGet("{monthlyExpensesId}")]
        [RequirePermission("EXr")]

        public async Task<ActionResult<List<ActionDto>>> GetActionsByMonthlyExpensesId(Guid monthlyExpensesId)
        {
            try
            {
                var query = new GetActionsByMonthlyExpensesIdQuery(monthlyExpensesId);
                var actions = await _mediator.Send(query);
                return Ok(actions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving actions.", details = ex.Message });
            }
        }

        [HttpPost]
        [RequirePermission("EXr")]
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
