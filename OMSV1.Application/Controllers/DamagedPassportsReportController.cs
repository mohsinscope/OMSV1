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
                _logger.LogInformation("Starting in-memory daily damaged passports archive generation.");

                // Convert the input UTC date to UTC+3 timezone for report generation
                var utcPlus3 = TimeSpan.FromHours(3);
                var localReportDay = request.ReportDate.Date;  // Get just the date part
                
                _logger.LogInformation("Generating report for date (UTC+3): {LocalReportDay}", localReportDay.ToString("yyyy-MM-dd"));

                var damagedPassports = await _damagedPassportRepository.GetDamagedPassportsByDateAsync(localReportDay);

                if (damagedPassports == null || !damagedPassports.Any())
                {
                    _logger.LogWarning("No damaged passports found for {LocalReportDay}", localReportDay.ToString("yyyy-MM-dd"));
                    return NotFound($"No damaged passports found for {localReportDay:yyyy-MM-dd}");
                }

                int totalPassports = damagedPassports.Count;
                _logger.LogInformation("Found {Count} damaged passports for day {LocalReportDay}", totalPassports, localReportDay.ToString("yyyy-MM-dd"));

                // Counter for the number of files added to the ZIP archive.
                int fileCountInZip = 0;

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

                                fileCountInZip++;
                            }
                            else
                            {
                                _logger.LogWarning("No file found for passport ID {PassportId} using pattern: DamagedPassport_{PassportId}_*.jpg", passport.Id, passport.Id);
                            }
                        }
                    }
                }

                // Ensure the number of files in the ZIP matches the number of damaged passports retrieved.
                if (fileCountInZip != totalPassports)
                {
                    _logger.LogError("Mismatch in file count: Retrieved {TotalPassports} passports but added {FileCount} files to the ZIP archive.", totalPassports, fileCountInZip);
                    return StatusCode(500, "Internal server error: Not all damaged passport files were found.");
                }

                // Reset the MemoryStream position to the beginning.
                archiveStream.Position = 0;

                // Convert the MemoryStream to a byte array.
                byte[] zipBytes = archiveStream.ToArray();

                // Create a filename based on the local (UTC+3) report day.
                string zipFileName = $"DamagedPassports_{localReportDay:yyyyMMdd}.zip";

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
            string baseFolder = @"\\172.16.108.26\samba";
            
            // Build a generic file search pattern that includes the passport ID.
            string fileSearchPattern = $"*{passport.Id}*.*";
            
            // Search recursively in all subdirectories of the base folder.
            var matches = Directory.GetFiles(baseFolder, fileSearchPattern, SearchOption.AllDirectories);
            
            if (matches.Length > 0)
            {
                return matches[0];
            }

            // Log if no file was found.
            _logger.LogWarning("No file found for passport ID {PassportId} using pattern: {Pattern}", passport.Id, fileSearchPattern);
            return string.Empty;
        }
    }
}
