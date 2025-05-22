using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Infrastructure.Services;

public class PhotoService : IPhotoService, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    // Use the generic repository for DamagedPassport
    private readonly IGenericRepository<DamagedPassport> _damagedPassportRepository;
    private readonly IGenericRepository<OMSV1.Domain.Entities.Documents.Document> _documentRepository;

    //private readonly string _networkStoragePath = @"\\172.16.108.26\samba";
    private readonly string _networkStoragePath = @"C:\Uploads";
    private const int MaxImageDimension = 1920; // Max dimension for images
    private const long MaxFileSize = 2048; // 2MB max file size
    private const int ImageQuality = 75; // JPEG quality (0-100)
    private const long MaxFolderSize = 50L * 1024L * 1024L * 1024L; // 50GB in bytes
    private readonly IMinioService _minioService;                  // ⬅ NEW


    public PhotoService(IWebHostEnvironment webHostEnvironment, IGenericRepository<DamagedPassport> damagedPassportRepository,IGenericRepository<OMSV1.Domain.Entities.Documents.Document> documentRepository, IMinioService minioService)
    {
        _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        _damagedPassportRepository = damagedPassportRepository ?? throw new ArgumentNullException(nameof(damagedPassportRepository));
        _documentRepository=documentRepository?? throw new ArgumentNullException(nameof(documentRepository));
        _minioService              = minioService ?? throw new ArgumentNullException(nameof(minioService)); // ⬅ NEW

    }
    

    private string GetNextAvailableFolder(string baseFolder)
    {
        int counter = 1;
        string currentFolder = baseFolder;

        // Keep checking folders until we find one under the size limit
        while (Directory.Exists(currentFolder) && GetDirectorySize(currentFolder) >= MaxFolderSize)
        {
            counter++;
            currentFolder = $"{baseFolder}{counter}";
        }

        Directory.CreateDirectory(currentFolder);
        return currentFolder;
    }

/* Simple helper; expand when you add more types */
private static string GetMimeType(string path) =>
    Path.GetExtension(path).ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg"           => "image/jpeg",
        ".png"                      => "image/png",
        ".pdf"                      => "application/pdf",
        _                           => "application/octet-stream"
    };

    private long GetDirectorySize(string folderPath)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
        return dirInfo.Exists ? 
            dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length) : 
            0;
    }
    

    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, Guid entityId, EntityType entityType)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded.");
            
            /* ─────────────────────────────────────────────────────
               1) Documents go straight to MinIO
               ────────────────────────────────────────────────────*/
            if (entityType == EntityType.Document)
            {
                // Delegate completely to the MinioService
                return await _minioService.AddPhotoAsync(file, entityId, entityType);
            }


        try
        {
            // Create base folder path for the entity type
            string entityTypeFolder = entityType.ToString().ToLower();
            string baseFolder = Path.Combine(_networkStoragePath, entityTypeFolder);
            
            // Get the appropriate folder to save the file
            string targetFolder = GetNextAvailableFolder(baseFolder);
            
            // Generate a unique filename
            string uniqueFileName;
            if (entityType == EntityType.Document)
            {
                // For Document, retrieve the document to get its DocumentNumber.
                // (Assumes you have an injected repository such as _documentRepository)
                var document = await _documentRepository.GetByIdAsync(entityId);
                if (document == null)
                    throw new Exception("Document not found for the provided entityId.");
                
                uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{document.DocumentNumber}_{DateTime.Now:yyyyMMdd}{Path.GetExtension(file.FileName)}";
            }
            else if (entityType == EntityType.DamagedPassport)
            {
                // Existing behavior for DamagedPassport remains unchanged.
                var damagedPassport = await _damagedPassportRepository.GetByIdAsync(entityId);
                if (damagedPassport == null)
                    throw new Exception("DamagedPassport not found for the provided entityId.");
                uniqueFileName = $"{entityType}__{entityId}_{damagedPassport.PassportNumber}_{file.FileName}";
            }
            else
            {
                // Default naming for other entity types.
                uniqueFileName = $"{entityType}_{entityId}_{Guid.NewGuid()}_{file.FileName}";
            }

            string filePath = Path.Combine(targetFolder, uniqueFileName);
            
            // Calculate the relative path from network storage root
            string relativePath = Path.GetRelativePath(_networkStoragePath, filePath);
            string fileUrl = $"/{relativePath.Replace('\\', '/')}" ;

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
                await CompressAndSaveFileAsync(file, filePath);
            }

            return new PhotoUploadResult
            {
                FilePath = fileUrl,
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
                leaveOpen: true))
        {
            await sourceStream.CopyToAsync(compressionStream);
        }

        // Check the compressed size
        if (memoryStream.Length > MaxFileSize)
        {
            throw new Exception("Compressed file size exceeds the maximum allowed size.");
        }

        // If the size is within the limit, write to the destination file
        memoryStream.Position = 0;
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
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty.", nameof(filePath));

            // If it's a MinIO URL (starts with /oms/ or whatever bucket), delegate to MinioService
            if (filePath.StartsWith("/oms/", StringComparison.OrdinalIgnoreCase))
                return await _minioService.DeletePhotoAsync(filePath);

            string full = Path.Combine(_networkStoragePath, filePath.TrimStart('/'));
            if (!File.Exists(full))
                throw new FileNotFoundException("File not found on network storage.", full);

            File.Delete(full);
            return true;
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
