using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace OMSV1.Infrastructure.Services;

public class PhotoService : IPhotoService, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _networkStoragePath = @"\\172.16.108.26\samba";
    private const int MaxImageDimension = 1920; // Max dimension for images
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB max file size
    private const int ImageQuality = 75; // JPEG quality (0-100)

    public PhotoService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
    }

    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, Guid entityId, EntityType entityType)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded.");

        try
        {
            // Ensure the uploads folder exists in network storage
            string uploadsFolder = Path.Combine(_networkStoragePath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            // Generate a unique filename
            string uniqueFileName = $"{entityType}_{entityId}_{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            string fileUrl = $"/uploads/{uniqueFileName}";

            // Process file based on its type
            if (IsImage(file.ContentType))
            {
                await OptimizeAndSaveImageAsync(file, filePath);
            }
            else if (IsPdf(file.ContentType))
            {
                await OptimizeAndSavePdfAsync(file, filePath);
            }
            else
            {
                // For other file types, just save with basic compression
                await CompressAndSaveFileAsync(file, filePath);
            }

            return new PhotoUploadResult
            {
                FilePath = fileUrl,
                FileName = uniqueFileName
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to upload and optimize file: {ex.Message}", ex);
        }
    }

    private async Task OptimizeAndSaveImageAsync(IFormFile file, string outputPath)
    {
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream());

        // Resize if necessary while maintaining aspect ratio
        if (image.Width > MaxImageDimension || image.Height > MaxImageDimension)
        {
            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new SixLabors.ImageSharp.Size(MaxImageDimension, MaxImageDimension)
            };
            image.Mutate(x => x.Resize(resizeOptions));
        }

        // Save with compression
        var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
        {
            Quality = ImageQuality
        };

        await image.SaveAsync(outputPath, encoder);
    }

    private async Task OptimizeAndSavePdfAsync(IFormFile file, string outputPath)
    {
        using (var inputStream = file.OpenReadStream())
        using (var outputStream = new FileStream(outputPath, FileMode.Create))
        {
            // Read the original PDF
            var reader = new PdfReader(inputStream);
            
            // Set up the stamper with compression
            var document = new Document();
            var copy = new PdfCopy(document, outputStream);
            document.Open();

            // Configure PDF settings for optimization
            copy.SetFullCompression();
            copy.CompressionLevel = PdfStream.BEST_COMPRESSION;

            // Copy each page with compression
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                var page = copy.GetImportedPage(reader, i);
                copy.AddPage(page);
            }

            // Clean up
            document.Close();
            reader.Close();
            copy.Close();
        }
    }

    private async Task CompressAndSaveFileAsync(IFormFile file, string outputPath)
    {
        using var sourceStream = file.OpenReadStream();
        using var destinationStream = new FileStream(outputPath, FileMode.Create);
        
        if (file.Length > MaxFileSize)
        {
            // Use basic compression for other file types
            using var compressionStream = new System.IO.Compression.GZipStream(
                destinationStream, 
                System.IO.Compression.CompressionLevel.Optimal);
            await sourceStream.CopyToAsync(compressionStream);
        }
        else
        {
            await sourceStream.CopyToAsync(destinationStream);
        }
    }

    private bool IsImage(string contentType)
    {
        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }

    private bool IsPdf(string contentType)
    {
        return contentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeletePhotoAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.");

        try
        {
            string fullFilePath = Path.Combine(_networkStoragePath, filePath.TrimStart('/'));
            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException("File not found in network storage.", fullFilePath);

            File.Delete(fullFilePath);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete file from network storage: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        // Clean up any resources
        GC.SuppressFinalize(this);
    }
}