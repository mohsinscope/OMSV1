using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSV1.Application.Helpers
{
    public class HangfireJobs
    {
        private readonly IMonthlyExpensesRepository _repository;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private readonly ILogger<HangfireJobs> _logger;
        private readonly IDamagedPassportRepository _damagedPassportRepository;
        private readonly IDamagedPassportService _damagedPassportService;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceService _attendanceService;
        private readonly IEmailReportRepository _emailReportRepository; // New dependency

        public HangfireJobs(
            IMonthlyExpensesRepository repository,
            IPdfService pdfService,
            IEmailService emailService,
            ILogger<HangfireJobs> logger,
            IDamagedPassportRepository damagedPassportRepository,
            IDamagedPassportService damagedPassportService,
            IAttendanceRepository attendanceRepository,
            IAttendanceService attendanceService,
            IEmailReportRepository emailReportRepository) // New parameter
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
