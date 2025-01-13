using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Enums;

namespace OMSV1.Infrastructure.Services;

public class PhotoService : IPhotoService, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _networkStoragePath = @"\\172.16.108.26\samba";
    // private readonly string _networkStoragePath = @"C:\Uploads";
    private const int MaxImageDimension = 1920; // Max dimension for images
    private const long MaxFileSize = 2048; // 2MB max file size
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
            var resizeOptions = new SixLabors.ImageSharp.Processing.ResizeOptions
            {
                Mode = SixLabors.ImageSharp.Processing.ResizeMode.Max,
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
        // Create temporary file for intermediate processing
        string tempPath = Path.GetTempFileName();
        
        try
        {
            // First, save the uploaded file to temp location
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using var outputStream = new FileStream(outputPath, FileMode.Create);
            using var reader = new PdfReader(tempPath);
            
            // Remove unused objects and compress objects
            reader.RemoveUnusedObjects();
            
            // Create new document with the same size as first page
            using var document = new Document(reader.GetPageSizeWithRotation(1));
            using var copy = new PdfSmartCopy(document, outputStream);
            
            // Enable compression
            copy.SetFullCompression();
            
            document.Open();

            // Clear metadata to reduce size
            reader.Info.Clear();

            // Process each page
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                var page = copy.GetImportedPage(reader, i);
                copy.AddPage(page);
            }

            document.Close();
            reader.Close();
            copy.Close();
        }
        finally
        {
            // Clean up temp file
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }

    private async Task CompressAndSaveFileAsync(IFormFile file, string outputPath)
    {
        using var sourceStream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();

        // Compress the file into a MemoryStream
        using (var compressionStream = new System.IO.Compression.GZipStream(
                memoryStream,
                System.IO.Compression.CompressionLevel.Optimal,
                leaveOpen: true)) // Leave the MemoryStream open
        {
            await sourceStream.CopyToAsync(compressionStream);
        }

        // Check the compressed size
        if (memoryStream.Length > MaxFileSize)
        {
            throw new Exception("Compressed file size exceeds the maximum allowed size.");
        }

        // If the size is within the limit, write to the destination file
        memoryStream.Position = 0; // Reset the stream position before writing
        using var destinationStream = new FileStream(outputPath, FileMode.Create);
        await memoryStream.CopyToAsync(destinationStream);
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
        GC.SuppressFinalize(this);
    }

    // public Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, Guid entityId, EntityType entityType)
    // {
    //     throw new NotImplementedException();
    // }
}
