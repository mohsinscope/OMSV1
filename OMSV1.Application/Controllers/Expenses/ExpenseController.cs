using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Authorization.Attributes;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Infrastructure.Extensions;

namespace OMSV1.Application.Controllers.Expenses
{
    public class ExpenseController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [RequirePermission("EXr")]
        public async Task<IActionResult> GetAllMonthlyExpenses([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllMonthlyExpensesQuery(paginationParams);
                var result = await _mediator.Send(query);

                // Add pagination header
                Response.AddPaginationHeader(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        [RequirePermission("EXr")]
        public async Task<IActionResult> GetMonthlyExpensesById(Guid id)
        {
            try
            {
                var query = new GetMonthlyExpensesByIdQuery(id);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the monthly expenses.", details = ex.Message });
            }
        }

        [HttpGet("{monthlyExpensesId}/daily-expenses")]
        [RequirePermission("EXr")]
        public async Task<IActionResult> GetDailyExpensesByMonthlyExpensesId(Guid monthlyExpensesId)
        {
            try
            {
                var result = await _mediator.Send(new GetDailyExpensesByMonthlyExpensesIdQuery(monthlyExpensesId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost ("MonthlyExpenses")]
        [RequirePermission("EXc")]
        public async Task<IActionResult> AddMonthlyExpenses([FromBody] CreateMonthlyExpensesCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { Id = id }); // Return 200 OK with the created ID
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        //Add Daily Expenses
        [HttpPost("{monthlyExpensesId}/daily-expenses")]
        [RequirePermission("EXc")]
        public async Task<IActionResult> AddDailyExpense(Guid monthlyExpensesId, [FromBody] AddDailyExpensesCommand command)
        {
            try
            {
                command.MonthlyExpensesId = monthlyExpensesId;
                var id = await _mediator.Send(command);
                return Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/status")]
        [RequirePermission("EXc")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateMonthlyExpensesStatusCommand command)
        {
            try
            {
                if (id != command.MonthlyExpensesId)
                {
                    return BadRequest("The ID in the URL does not match the ID in the request body.");
                }

                var result = await _mediator.Send(command);
                return result ? NoContent() : BadRequest("Failed to update status.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("search")]
        [RequirePermission("EXr")]
        public async Task<IActionResult> SearchMonthlyExpenses([FromBody] GetFilteredMonthlyExpensesQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result); // Add pagination metadata
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("search-statistics")]
        [RequirePermission("EXr")]
        public async Task<IActionResult> SearchStatistics([FromBody] SearchExpensesStatisticsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, new
                {
                    message = "An error occurred while retrieving expenses statistics.",
                    details = ex.Message
                });
            }
        }
        [HttpPost("compare-statistics")]
        [RequirePermission("EXr")]
        public async Task<IActionResult> CompareStatistics([FromBody] CompareMonthlyExpensesQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, new
                {
                    message = "An error occurred while comparing monthly expenses statistics.",
                    details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [RequirePermission("EXu")]
        public async Task<IActionResult> UpdateDailyExpense(Guid id, [FromBody] UpdateDailyExpensesCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Mismatched DailyExpenses ID.");

                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound($"DailyExpenses with ID {id} not found.");

                return NoContent(); // Return 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission("EXd")]
        public async Task<IActionResult> DeleteDailyExpense(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteDailyExpensesCommand(id));

                if (!result)
                {
                    return NotFound($"DailyExpenses with ID {id} not found.");
                }

                return NoContent(); // Return 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
