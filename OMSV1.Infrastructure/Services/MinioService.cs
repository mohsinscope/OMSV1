using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace OMSV1.Infrastructure.Services
{
    public class MinioService : IMinioService, IDisposable
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<PhotoService> _logger;
        private readonly string _bucketName = "oms"; // Change as needed

        private const int MaxImageDimension = 1920;
        private const int ImageQuality = 75;

        public MinioService(ILogger<PhotoService> logger)
        {
            _logger = logger;

            _minioClient = new MinioClient()
                .WithEndpoint("192.168.108.32", 9000)
                .WithCredentials("iD7LXOOArqDRYpMmHN3U", "MKBEwY9ofM9UhIsmSwDc1qYlQqawXR1pSQXpdpAc")
                .WithSSL(false)
                .Build();

            EnsureBucketExistsAsync().GetAwaiter().GetResult();
        }

        private async Task EnsureBucketExistsAsync()
        {
            bool exists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!exists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }
        }

        public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, Guid entityId, EntityType entityType)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var objectName = $"{entityType}/{entityId}_{Guid.NewGuid()}_{file.FileName}";

            try
            {
                if (IsImage(file.ContentType))
                {
                    using var memoryStream = new MemoryStream();
                    using var image = await Image.LoadAsync(file.OpenReadStream());

                    if (image.Width > MaxImageDimension || image.Height > MaxImageDimension)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(MaxImageDimension, MaxImageDimension)
                        }));
                    }

                    var encoder = new JpegEncoder { Quality = ImageQuality };
                    await image.SaveAsync(memoryStream, encoder);
                    memoryStream.Position = 0;

                    await _minioClient.PutObjectAsync(new PutObjectArgs()
                        .WithBucket(_bucketName)
                        .WithObject(objectName)
                        .WithContentType("image/jpeg")
                        .WithStreamData(memoryStream)
                        .WithObjectSize(memoryStream.Length));

                    return new PhotoUploadResult
                    {
                        FilePath = $"/{_bucketName}/{objectName}"
                    };
                }
                else if (IsPdf(file.ContentType))
                {
                    // Handle PDF optimization if needed
                    await UploadDirectlyAsync(file, objectName, file.ContentType);
                }
                else
                {
                    await UploadDirectlyAsync(file, objectName, file.ContentType);
                }

                return new PhotoUploadResult
                {
                    FilePath = $"/{_bucketName}/{objectName}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file.");
                throw;
            }
        }

        private async Task UploadDirectlyAsync(IFormFile file, string objectName, string contentType)
        {
            var tempFilePath = Path.GetTempFileName();
            try
            {
                using var stream = new FileStream(tempFilePath, FileMode.Create);
                await file.CopyToAsync(stream);

                var fileInfo = new FileInfo(tempFilePath);
                using var fileStream = File.OpenRead(tempFilePath);

                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithContentType(contentType)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileInfo.Length));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        public async Task<bool> DeletePhotoAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be empty.");

            var parts = filePath.TrimStart('/').Split('/', 2);
            var bucket = parts[0];
            var objectName = parts[1];

            try
            {
                await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectName));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete object: {objectName}");
                return false;
            }
        }

        private bool IsImage(string contentType) =>
            contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);

        private bool IsPdf(string contentType) =>
            contentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}