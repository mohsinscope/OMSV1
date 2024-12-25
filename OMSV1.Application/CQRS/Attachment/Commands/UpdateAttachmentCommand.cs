using MediatR;
using Microsoft.AspNetCore.Http;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Attachment
        {
            public class UpdateAttachmentCommand : IRequest<AttachmentDto>
            {
                public int Id { get; set; }
                public IFormFile File { get; set; }
                public string FileName { get; set; }
                public EntityType EntityType { get; set; }
                public int EntityId { get; set; }

                public UpdateAttachmentCommand(int id, IFormFile file, string fileName, EntityType entityType, int entityId)
                {
                    Id = id;
                    File = file;
                    FileName = fileName;
                    EntityType = entityType;
                    EntityId = entityId;
                }
            }
        }
