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
    private readonly string _networkStoragePath = @"\\172.16.108.26\samba";

    public PhotoService(IWebHostEnvironment webHostEnvironment)
    {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));

    }
    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, int entityId, EntityType entityType)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded.");

        try
        {
            // Ensure the uploads folder exists in network storage
            string uploadsFolder = Path.Combine(_networkStoragePath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate a unique filename based on entity type and entity ID
            string uniqueFileName = $"{entityType}_{entityId}_{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            string fileUrl = $"/uploads/{uniqueFileName}"; // Keep the URL format for web access

            // Save file to network storage
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
        catch (Exception ex)
        {
            // Add proper error handling
            throw new Exception($"Failed to upload file to network storage: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeletePhotoAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.");

        try
        {
            // Construct the full file path in network storage
            string fullFilePath = Path.Combine(_networkStoragePath, filePath.TrimStart('/'));

            // Check if the file exists
            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException("File not found in network storage.", fullFilePath);

            // Delete the file
            File.Delete(fullFilePath);

            return true; // Return true if file deletion is successful
        }
        catch (Exception ex)
        {
            // Add proper error handling
            throw new Exception($"Failed to delete file from network storage: {ex.Message}", ex);
        }
    }


}
