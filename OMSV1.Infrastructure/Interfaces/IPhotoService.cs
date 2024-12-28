using System;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Enums;

namespace OMSV1.Infrastructure.Interfaces;

public interface IPhotoService
{
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file,int entityId, EntityType  entityType);
    Task<bool> DeletePhotoAsync(string publicId);
}

