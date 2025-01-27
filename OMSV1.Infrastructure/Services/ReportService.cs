using OMSV1.Domain.Entities.Expenses;
using OMSV1.Infrastructure.Services;

public class ReportService
{
    private readonly EmailService _emailService;
    private readonly ITextSharpPdfService _pdfService;

    public ReportService(EmailService emailService, ITextSharpPdfService pdfService)
    {
        _emailService = emailService;
        _pdfService = pdfService;
    }

   public async Task SendMonthlyReportAsync(List<MonthlyExpenses> expenses)
{
    var pdfData = await _pdfService.GenerateMonthlyExpensesPdfAsync(expenses);

    await _emailService.SendEmailAsync(
        from: "yass39656@gmail.com",
        to: "yass22185@gmail.com",
        subject: "Monthly Expenses Report",
        body: "Please find the monthly expenses report attached.",
        pdfData: pdfData
    );
}

public async Task SendTestEmail()
{
    await _emailService.SendEmailAsync(
        from: "yass39656@gmail.com", // Hardcode or pass as parameter
        to: "yass22185@gmail.com",
        subject: "Test Email",
        body: "Hello World!"
    );
}
}