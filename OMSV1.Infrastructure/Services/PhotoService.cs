using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Infrastructure.Services;

public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public PhotoService(IWebHostEnvironment webHostEnvironment)
    {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));


    }
    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, int entityId, EntityType entityType)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded.");

        // Determine the web root path
        string webRootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        // Ensure the uploads folder exists
        string uploadsFolder = Path.Combine(webRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Generate a unique filename based on entity type and entity ID
        string uniqueFileName = $"{entityType}_{entityId}_{file.FileName}";
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        string fileUrl = $"/uploads/{uniqueFileName}";

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return new PhotoUploadResult
        {
            FilePath = fileUrl,
            FileName = uniqueFileName
        };
    }

    public Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        throw new NotImplementedException();
    }


    // public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    // {
    //     var deleteParams = new DeletionParams(publicId);
    //     return await _cloudinary.DestroyAsync(deleteParams);
    // }
}
