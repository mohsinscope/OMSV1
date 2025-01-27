using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace OMSV1.Application.Helpers;

public class HangfireJobs
{
    private readonly IMonthlyExpensesRepository _repository;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;
    private readonly ILogger<HangfireJobs> _logger;

    public HangfireJobs(
        IMonthlyExpensesRepository repository,
        IPdfService pdfService,
        IEmailService emailService,
        ILogger<HangfireJobs> logger)
    {
        _repository = repository;
        _pdfService = pdfService;
        _emailService = emailService;
        _logger = logger;
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
            // Generate the PDF as a byte array
            var pdfData = await _pdfService.GenerateMonthlyExpensesPdfAsync(expenses);
            _logger.LogInformation("PDF generated successfully");

            // Recipients list
            var recipients = new[] { "yass22185@gmail.com", "ali.yaas@scopesky.iq" };

            // Send the email with the PDF attached to multiple recipients
            foreach (var recipient in recipients)
            {
                await _emailService.SendEmailAsync(
                    from: "yass39656@gmail.com",
                    to: recipient,
                    subject: "تقرير الصرفيات الشهري",
                    body: "المصاريف الشهرية",
                    pdfData: pdfData // Pass byte array directly
                );

                _logger.LogInformation($"Monthly expenses report email sent successfully to {recipient}.");
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


}
