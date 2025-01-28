using System;
using Microsoft.AspNetCore.Http;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Enums;

namespace OMSV1.Infrastructure.Interfaces;

public interface IPhotoService
{
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, Guid entityId, EntityType entityType);
    Task<bool> DeletePhotoAsync(string filePath);
}

