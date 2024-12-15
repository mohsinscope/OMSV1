using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace OMSV1.Application.Commands.Attachment{
public class AddAttachmentsToEntityCommand : IRequest<bool>
{
    public string EntityName { get; set; } // E.g., "DamagedDevice"
    public int EntityId { get; set; } // ID of the specific entity

    [FromForm]
    public List<IFormFile> Files { get; set; } // List of files to upload
}
}