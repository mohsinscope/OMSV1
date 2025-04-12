using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Projects;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Projects;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Projects
{
    public class ProjectController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }
                // GET: api/project?PageNumber=1&PageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAllProjects([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllProjectsQuery(paginationParams);
                var projects = await _mediator.Send(query);
                
                // Add pagination details to the response headers
                Response.AddPaginationHeader(projects);
                
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving projects.",
                    new[] { ex.Message }
                );
            }
        }
        // GET: api/project/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            try
            {
                var query = new GetProjectByIdQuery { Id = id };
                var project = await _mediator.Send(query);
                return Ok(project);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the project by ID.",
                    new[] { ex.Message }
                );
            }
        }

        // POST: Add a new Project
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] AddProjectCommand command)
        {
            try
            {
                var projectId = await _mediator.Send(command);
                return Ok(new { Message = "Project created successfully", Id = projectId });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while creating the project.",
                    new[] { ex.Message }
                );
            }
        }
                // PUT: api/project/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    return BadRequest("Project ID mismatch.");
                }

                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok("Project updated successfully.");
                }
                else
                {
                    return BadRequest("Failed to update the project.");
                }
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the project.",
                    new[] { ex.Message }
                );
            }
        }
        // DELETE: api/project/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                var command = new DeleteProjectCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok("Project deleted successfully.");
                }
                return BadRequest("Failed to delete the project.");
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the project.",
                    new[] { ex.Message }
                );
            }
        }
    }
}
