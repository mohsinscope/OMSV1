using System;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Dtos;

public class AttachmentDto
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public EntityType EntityType { get; set; }
    public int EntityId { get; set; }
}

