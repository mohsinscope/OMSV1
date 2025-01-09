using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Infrastructure.Extensions;
using System.Net;
namespace OMSV1.Application.Controllers.LOV;

public class ExpenseTypeController : BaseApiController
{
    private readonly IMediator _mediator;

    public ExpenseTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Get All ExpenseTypes with Pagination
// POST method to add a new ExpenseType
        [HttpPost]
        public async Task<IActionResult> AddExpenseType([FromBody] CreateExpenseTypeCommand command)
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
        [HttpGet]
        public async Task<IActionResult> GetAllExpenseTypes([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                // Create and send the query to retrieve paginated expense types
                var query = new GetAllExpenseTypesQuery(paginationParams);
                var expenseTypes = await _mediator.Send(query);

                // Add pagination headers to the response
                Response.AddPaginationHeader(expenseTypes);

                // Return the paginated result
                return Ok(expenseTypes);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving expense types.", 
                    new[] { ex.Message }
                );
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpenseType(Guid id, [FromBody] UpdateExpenseTypeCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("The ID in the URL does not match the ID in the request body.");

                var isUpdated = await _mediator.Send(command);

                if (!isUpdated)
                    return NotFound($"ExpenseType with ID {id} was not found.");

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the expense type.",
                    new[] { ex.Message }
                );
            }
        }
          // DELETE method to delete an ExpenseType
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseType(Guid id)
        {
            try
            {
                var command = new DeleteExpenseTypeCommand(id);
                var isDeleted = await _mediator.Send(command);

                if (!isDeleted)
                    return NotFound($"ExpenseType with ID {id} not found.");

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while deleting the expense type.", new[] { ex.Message });
            }
        }





}
