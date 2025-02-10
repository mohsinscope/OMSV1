using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace OMSV1.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamagedPassportsReportController : ControllerBase
    {
        private readonly ILogger<DamagedPassportsReportController> _logger;
        private readonly IDamagedPassportRepository _damagedPassportRepository;
        private readonly IDamagedPassportService _damagedPassportService;

        public DamagedPassportsReportController(
            ILogger<DamagedPassportsReportController> logger,
            IDamagedPassportRepository damagedPassportRepository,
            IDamagedPassportService damagedPassportService)
        {
            _logger = logger;
            _damagedPassportRepository = damagedPassportRepository;
            _damagedPassportService = damagedPassportService;
        }

        [HttpPost("zip")]
        [Authorize(Policy = "RequireSuperAdminRole")]
        public async Task<IActionResult> DownloadDailyDamagedPassportsZipArchiveReport([FromBody] DamagedPassportsReportRequest request)
        {
            try
            {
                _logger.LogInformation("Starting in-memory daily damaged passports archive generation");

                // Use the report date provided in the POST body (date-only).
                var reportDate = request.ReportDate.Date;

                // Retrieve passports created on the specified date.
                var damagedPassports = await _damagedPassportRepository.GetDamagedPassportsByDateAsync(reportDate);

                if (damagedPassports == null || !damagedPassports.Any())
                {
                    _logger.LogWarning($"No damaged passports found for {reportDate:yyyy-MM-dd}");
                    return NotFound($"No damaged passports found for {reportDate:yyyy-MM-dd}");
                }

                // Create a MemoryStream to hold the ZIP archive.
                using var archiveStream = new MemoryStream();

                // Create a ZIP archive within the MemoryStream.
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    // Group passports by their DamageType name.
                    var groups = damagedPassports.GroupBy(dp => dp.DamagedType.Name);

                    foreach (var group in groups)
                    {
                        // Use the damage type name as the folder name inside the ZIP.
                        string folderName = group.Key;

                        foreach (var passport in group)
                        {
                            // Retrieve the file path for the attachment using your custom logic.
                            string filePath = GetAttachmentFilePath(passport);

                            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                            {
                                string fileName = Path.GetFileName(filePath);

                                // Build an entry name that places the file within a folder (damage type name).
                                string entryName = $"{folderName}/{fileName}";

                                // Create an entry in the ZIP archive.
                                var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);

                                // Open the entry stream and copy the file content.
                                using var entryStream = entry.Open();
                                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                await fileStream.CopyToAsync(entryStream);
                            }
                            else
                            {
                                _logger.LogWarning($"No file found for passport ID {passport.Id} using pattern: DamagedPassport_{passport.Id}_*.jpg");
                            }
                        }
                    }
                }

                // Reset the MemoryStream position to the beginning.
                archiveStream.Position = 0;

                // Convert the MemoryStream to a byte array.
                byte[] zipBytes = archiveStream.ToArray();

                // Create a filename based on the report date.
                string zipFileName = $"DamagedPassports_{reportDate:yyyyMMdd}.zip";

                // Return the ZIP archive as a downloadable file.
                return File(zipBytes, "application/zip", zipFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating daily damaged passports archive report");
                return StatusCode(500, "Internal server error");
            }
        }

        // Helper method for determining the attachment file path.
        private string GetAttachmentFilePath(Domain.Entities.DamagedPassport.DamagedPassport passport)
        {
            // Define the base folder where all entity folders are stored.
            string baseFolder = @"C:\Uploads";
            
            // Use a search pattern to get all folders starting with "damagedpassport"
            string folderSearchPattern = "damagedpassport*";
            string[] directories = Directory.GetDirectories(baseFolder, folderSearchPattern, SearchOption.TopDirectoryOnly);

            // Build a file search pattern using the passport ID.
            string fileSearchPattern = $"DamagedPassport_{passport.Id}_*.*";

            // Loop through each matching directory and search for the file.
            foreach (string directory in directories)
            {
                // Search only within the current directory.
                string[] matches = Directory.GetFiles(directory, fileSearchPattern, SearchOption.TopDirectoryOnly);
                if (matches.Length > 0)
                {
                    // Return the first found file.
                    return matches[0];
                }
            }

            // Log if no file was found in any folder.
            _logger.LogWarning($"No file found for passport ID {passport.Id} using pattern: {fileSearchPattern}");
            return string.Empty;
        }
    }
}
