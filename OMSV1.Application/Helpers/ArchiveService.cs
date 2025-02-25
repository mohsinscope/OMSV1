using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;

public class DamagedPassportJobService
{
    private readonly ILogger<DamagedPassportJobService> _logger;
    private readonly IDamagedPassportRepository _damagedPassportRepository;
    private readonly IEmailReportRepository _emailReportRepository;
    private readonly IEmailService _emailService;

    // 24 MB in bytes
    private const long MaxAttachmentSizeBytes = 24L * 1024L * 1024L;

    public DamagedPassportJobService(
        ILogger<DamagedPassportJobService> logger,
        IDamagedPassportRepository damagedPassportRepository,
        IEmailReportRepository emailReportRepository,
        IEmailService emailService)
    {
        _logger = logger;
        _damagedPassportRepository = damagedPassportRepository;
        _emailReportRepository = emailReportRepository;
        _emailService = emailService;
    }

    /// <summary>
    /// Hangfire job: Generates a daily damaged passports ZIP archive (from the previous day in UTC+3)
    /// and emails it to the configured recipients. Ensures compression and checks if final size exceeds 24 MB.
    /// </summary>
    public async Task GenerateAndSendDailyDamagedPassportsZipArchiveReport()
    {
        try
        {
            _logger.LogInformation("Starting daily damaged passports archive generation via Hangfire.");

            // Convert current UTC time to UTC+3 and then get yesterday's date.
            var localNow = DateTime.UtcNow.AddHours(3);
            var localReportDay = localNow.Date.AddDays(-1);

            _logger.LogInformation("Generating report for date (UTC+3): {LocalReportDay}", localReportDay.ToString("yyyy-MM-dd"));

            // Fetch passports for that day
            var damagedPassports = await _damagedPassportRepository.GetDamagedPassportsByDateAsync(localReportDay);
            if (damagedPassports == null || !damagedPassports.Any())
            {
                _logger.LogWarning("No damaged passports found for {LocalReportDay}", localReportDay.ToString("yyyy-MM-dd"));
                return;
            }

            int totalPassports = damagedPassports.Count;
            _logger.LogInformation("Found {Count} damaged passports for day {LocalReportDay}", totalPassports, localReportDay.ToString("yyyy-MM-dd"));

            int fileCountInZip = 0;

            using var archiveStream = new MemoryStream();

            // Create a ZIP archive with compression
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                // Group passports by their DamageType name
                var groups = damagedPassports.GroupBy(dp => dp.DamagedType.Name);

                foreach (var group in groups)
                {
                    // Folder name inside the ZIP
                    string folderName = group.Key;

                    foreach (var passport in group)
                    {
                        // Retrieve the file path for the attachment
                        string filePath = GetAttachmentFilePath(passport);

                        // If file exists, add it to the ZIP
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            string fileName = Path.GetFileName(filePath);
                            string entryName = $"{folderName}/{fileName}";

                            // Use compression to minimize final size
                            var entry = archive.CreateEntry(entryName, CompressionLevel.SmallestSize);

                            using var entryStream = entry.Open();
                            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                            await fileStream.CopyToAsync(entryStream);

                            fileCountInZip++;
                        }
                        else
                        {
                            _logger.LogWarning(
                                "No file found for passport ID {PassportId} when attempting to create ZIP entry.",
                                passport.Id
                            );
                        }
                    }
                }
            }

            // Log if there's a mismatch between passports and files
            if (fileCountInZip != totalPassports)
            {
                _logger.LogError(
                    "Mismatch in file count: Retrieved {TotalPassports} passports but added {FileCount} files to ZIP for date {LocalReportDay}",
                    totalPassports, fileCountInZip, localReportDay.ToString("yyyy-MM-dd")
                );
            }

            // Convert archive to byte array
            archiveStream.Position = 0;
            var zipBytes = archiveStream.ToArray();

            // Check final size before emailing
            if (zipBytes.Length > MaxAttachmentSizeBytes)
            {
                var sizeInMB = (zipBytes.Length / 1024f) / 1024f;
                _logger.LogWarning(
                    "The resulting ZIP ({SizeInMB:F2} MB) exceeds the 24 MB limit. " +
                    "This may cause email delivery issues or require alternative handling.",
                    sizeInMB
                );

                // TODO: Optionally handle a "split zip" or skip sending if desired.
            }

            // Prepare email
            string emailBody = "Please find attached the daily damaged passports archive.";

            var recipients = await _emailReportRepository.GetEmailsByReportTypeAsync("Daily Passports Archives");
            if (recipients != null && recipients.Any())
            {
                foreach (var recipient in recipients)
                {
                    await _emailService.SendEmailWithAttachmentAsync(
                        from: "omc@scopesky.iq",
                        to: recipient,
                        subject: $"Daily Damaged Passports Archive - {localReportDay:yyyyMMdd}",
                        body: emailBody,
                        attachmentBytes: zipBytes,
                        attachmentName: $"DamagedPassports_{localReportDay:yyyyMMdd}.zip"
                    );

                    _logger.LogInformation(
                        "Daily damaged passports archive email sent successfully to {Recipient}.",
                        recipient
                    );
                }
            }
            else
            {
                _logger.LogWarning("No recipients found for report type 'Daily Passports Archives'");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating or sending daily damaged passports archive report");
            throw; // Let Hangfire handle the exception
        }
    }

    /// <summary>
    /// Helper method for determining the attachment file path for a given DamagedPassport.
    /// Update to match your file-storage logic (same as in the controller).
    /// </summary>
    private string GetAttachmentFilePath(OMSV1.Domain.Entities.DamagedPassport.DamagedPassport passport)
    {
        string baseFolder = @"\\172.16.108.26\samba";
        string fileSearchPattern = $"*{passport.Id}*.*";

        var matches = Directory.GetFiles(baseFolder, fileSearchPattern, SearchOption.AllDirectories);
        if (matches.Length > 0)
        {
            return matches[0];
        }

        _logger.LogWarning(
            "No file found for passport ID {PassportId} using pattern: {Pattern}",
            passport.Id,
            fileSearchPattern
        );
        return string.Empty;
    }
}
