using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Department;
using OMSV1.Application.Commands.Departments;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Departments;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Documents
{
    public class DepartmentController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
                // Get all Department with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllDepartmentsQuery(paginationParams);
                var Department = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(Department);

                return Ok(Department);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving Department.",
                    new[] { ex.Message }
                );
            }
        }

        // // // GET: api/Department/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetDepartmentsByIdQuery(id);
                var Department = await _mediator.Send(query);

                return Ok(Department);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the Department by ID.", 
                    new[] { ex.Message }
                );
            }
        }
        // GET: api/Departments/ByDirectorate/{directorateId}
[HttpGet("ByDirectorate/{directorateId}")]
public async Task<IActionResult> GetByDirectorate(Guid directorateId)
{
    try
    {
        var query = new GetDepartmentsByDirectorateIdQuery(directorateId);
        var departments = await _mediator.Send(query);

        if (!departments.Any())
            return NotFound($"No departments found for DirectorateId {directorateId}.");

        return Ok(departments);
    }
    catch (ArgumentException argEx)
    {
        return BadRequest(argEx.Message);
    }
    catch (Exception ex)
    {
        return ResponseHelper.CreateErrorResponse(
            HttpStatusCode.InternalServerError,
            "An error occurred while retrieving departments by DirectorateId.",
            new[] { ex.Message }
        );
    }
}


        

        // Add a new Department
        [HttpPost]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = " Department added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the Department.", 
                    new[] { ex.Message }
                );
            }
        }
        //                 // PUT: Update an existing Department
        [HttpPut("{id}")]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Department ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Department updated successfully.");
                else
                    return BadRequest("Failed to update Department.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the Department.",
                    new[] { ex.Message }
                );
            }
        }

        //         // DELETE: Delete a Department
        [HttpDelete("{id}")]
        //[RequirePermission("LOVDOC")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                var command = new DeleteDepartmentCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Department deleted successfully.");
                else
                    return BadRequest("Failed to delete Department.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the Department .",
                    new[] { ex.Message }
                );
            }
        }
     

    }
}
