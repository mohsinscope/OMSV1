using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;
using OMSV1.Domain.Entities.DamagedPassport;

namespace OMSV1.Application.Helpers
{
    public class HangfireJobs
    {
        private readonly IMonthlyExpensesRepository _repository;
        private readonly IConfiguration _configuration;

        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private readonly ILogger<HangfireJobs> _logger;
        private readonly IDamagedPassportRepository _damagedPassportRepository;
        private readonly IDamagedPassportService _damagedPassportService;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceService _attendanceService;
        private readonly IEmailReportRepository _emailReportRepository; // New dependency
        private readonly IDamagedPassportArchiveService _archiveService;


        public HangfireJobs(
            IMonthlyExpensesRepository repository,
            IPdfService pdfService,
            IEmailService emailService,
            ILogger<HangfireJobs> logger,
            IDamagedPassportRepository damagedPassportRepository,
            IDamagedPassportService damagedPassportService,
            IAttendanceRepository attendanceRepository,
            IAttendanceService attendanceService,
            IEmailReportRepository emailReportRepository,
            IDamagedPassportArchiveService archiveService,
            IConfiguration configuration) // New parameter
        {
            _repository = repository;
            _pdfService = pdfService;
            _emailService = emailService;
            _logger = logger;
            _damagedPassportRepository = damagedPassportRepository;
            _damagedPassportService = damagedPassportService;
            _attendanceRepository = attendanceRepository;
            _attendanceService = attendanceService;
            _emailReportRepository = emailReportRepository; // Assign the new dependency
            _archiveService = archiveService;
            _configuration = configuration;


        }

        public async Task GenerateAndSendMonthlyExpensesReport()
        {
            try
            {
                _logger.LogInformation("Starting monthly expenses report generation and email sending");

                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddDays(-30);

                var expenses = await _repository.GetExpensesByDateRangeAsync(startDate, endDate);

                if (expenses != null && expenses.Any())
                {
                    var pdfData = await _pdfService.GenerateMonthlyExpensesPdfAsync(expenses);
                    _logger.LogInformation("PDF generated successfully");

                    // Fetch recipients based on report type "Incident Report"
                    var recipients = await _emailReportRepository.GetEmailsByReportTypeAsync("Monthly Expenses");

                    if (recipients != null && recipients.Any())
                    {
                        foreach (var recipient in recipients)
                        {
                            await _emailService.SendEmailAsync(
                                from: "omc@scopesky.iq",
                                to: recipient,
                                subject: "تقرير الصرفيات الشهري",
                                body: "المصاريف الشهرية",
                                pdfData: pdfData
                            );

                            _logger.LogInformation($"Monthly expenses report email sent successfully to {recipient}.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No recipients found for report type 'Incident Report'");
                    }
                }
                else
                {
                    _logger.LogWarning("No expenses found for the last 30 days");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating or sending monthly expenses report");
                throw;
            }
        }
        public async Task GenerateAndSendDailyDamagedPassportsZipArchiveReport()
        {
            try
            {
                _logger.LogInformation("Starting in-memory daily damaged passports archive generation");

                // Get today's damaged passports.
                var today = DateTime.UtcNow.Date;
                var damagedPassports = await _damagedPassportRepository.GetDamagedPassportsByDateAsync(today);

                if (damagedPassports == null || !damagedPassports.Any())
                {
                    _logger.LogWarning("No damaged passports found for today");
                    return;
                }

                // Create a MemoryStream to hold the ZIP archive.
                using var archiveStream = new MemoryStream();

                // Create a ZIP archive within the MemoryStream.
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    // Group passports by their DamagedTypeId.
                    var groups = damagedPassports.GroupBy(dp => dp.DamagedTypeId);

                    foreach (var group in groups)
                    {
                        // Use the damage type as the folder name inside the ZIP.
                        string folderName = group.Key.ToString();

                        foreach (var passport in group)
                        {
                            // Retrieve the file path for the attachment.
                            // (Implement this method based on how you store or reference files for damaged passports.)
                            string filePath = GetAttachmentFilePath(passport);

                            if (File.Exists(filePath))
                            {
                                string fileName = Path.GetFileName(filePath);

                                // Build an entry name that places the file within a folder (damage type).
                                string entryName = $"{folderName}/{fileName}";

                                // Create an entry in the ZIP archive.
                                var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);

                                // Open the entry stream and copy the file content.
                                using var entryStream = entry.Open();
                                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                await fileStream.CopyToAsync(entryStream);
                            }
                        }
                    }
                }

                // Reset the MemoryStream position to the beginning.
                archiveStream.Position = 0;

                // Convert the MemoryStream to a byte array.
                byte[] zipBytes = archiveStream.ToArray();

                // Prepare a simple email body.
                string emailBody = "Please find attached the daily damaged passports archive.";

                // Retrieve recipients for the "Daily Passports Archives" report type.
                var recipients = await _emailReportRepository.GetEmailsByReportTypeAsync("Daily Passports Archives");

                if (recipients != null && recipients.Any())
                {
                    // For each recipient, send an email with the ZIP archive attached.
                    foreach (var recipient in recipients)
                    {
                   await _emailService.SendEmailWithAttachmentAsync(
                    from: "omc@scopesky.iq",
                    to: recipient,
                    subject: $"Daily Damaged Passports Archive - {today:yyyyMMdd}",
                    body: emailBody,
                    attachmentBytes: zipBytes,
                    attachmentName: $"DamagedPassports_{today:yyyyMMdd}.zip"
                );

                        _logger.LogInformation($"Daily damaged passports archive email sent successfully to {recipient}.");
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
                throw;
            }
        }

        /// <summary>
        /// Dummy helper method to obtain the file path for a damaged passport attachment.
        /// Replace this with your actual logic to locate the file.
        /// </summary>
   private string GetAttachmentFilePath(Domain.Entities.DamagedPassport.DamagedPassport passport)
{
    // Define the folder where the images are stored.
    string folder = @"C:\Uploads\damagedpassport";

    // Build a search pattern using the passport ID.
    // For example, if files start with "DamagedPassport_{passport.Id}_"
    // and then some extra info, use a wildcard after the ID.
    string pattern = $"DamagedPassport_{passport.Id}_*.jpg";

    // Get matching files in the folder.
    string[] matches = Directory.GetFiles(folder, pattern);

    // If at least one match is found, return the first match.
    if (matches.Length > 0)
    {
        return matches[0];
    }
    else
    {
        // Optionally log that no file was found for this passport.
        Console.WriteLine($"No file found for passport ID {passport.Id} using pattern: {pattern}");
        return string.Empty;
    }
}


        public async Task GenerateAndSendDailyAttendanceReport()
        {
            try
            {
                _logger.LogInformation("Starting daily attendance report generation and email sending");

                // Get today's date
                var today = DateTime.UtcNow.Date;

                // Fetch attendance records for today
                var attendances = await _attendanceRepository.GetAttendanceByDateAsync(today);

                if (attendances != null && attendances.Any())
                {
                    // Generate the PDF as a byte array
                    var pdfData = await _attendanceService.GenerateDailyAttendancePdfAsync(attendances);
                    _logger.LogInformation("PDF generated successfully");

                    // Fetch recipients based on report type "Incident Report"
                    var recipients = await _emailReportRepository.GetEmailsByReportTypeAsync("Daily Attendances");

                    if (recipients != null && recipients.Any())
                    {
                        foreach (var recipient in recipients)
                        {
                            await _emailService.SendEmailAsync(
                                from: "omc@scopesky.iq",
                                to: recipient,
                                subject: "تقرير الحضور اليومي",
                                body: "تقرير الحضور اليومي مرفق",
                                pdfData: pdfData
                            );

                            _logger.LogInformation($"Daily attendance report email sent successfully to {recipient}.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No recipients found for report type 'Incident Report'");
                    }
                }
                else
                {
                    _logger.LogWarning("No attendance records found for today.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating or sending daily attendance report");
                throw;
            }
        }

        public async Task GenerateAndSendDailyDamagedPassportsReport()
        {
            try
            {
                _logger.LogInformation("Starting daily damaged passports report generation and email sending");

                var today = DateTime.UtcNow.Date;
                var damagedPassports = await _damagedPassportRepository.GetDamagedPassportsByDateAsync(today);

                if (damagedPassports != null && damagedPassports.Any())
                {
                    var pdfData = await _damagedPassportService.GenerateDailyDamagedPassportsPdfAsync(damagedPassports);
                    _logger.LogInformation("PDF generated successfully");

                    // Fetch recipients based on report type "Incident Report"
                    var recipients = await _emailReportRepository.GetEmailsByReportTypeAsync("Daily Passports");

                    if (recipients != null && recipients.Any())
                    {
                        foreach (var recipient in recipients)
                        {
                            await _emailService.SendEmailAsync(
                                from: "omc@scopesky.iq",
                                to: recipient,
                                subject: "تقرير الجوازات التالفة اليومية",
                                body: "تقرير الجوازات التالفة المسجلة اليوم",
                                pdfData: pdfData
                            );

                            _logger.LogInformation($"Daily damaged passports report email sent successfully to {recipient}.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No recipients found for report type 'Incident Report'");
                    }
                }
                else
                {
                    _logger.LogWarning("No damaged passports found for today");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating or sending daily damaged passports report");
                throw;
            }
        }
    }
}
