using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Controllers;
using OMSV1.Application.Helpers;

[Authorize(Roles = "SuperAdmin,Admin")]
public class ReportsController : BaseApiController
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ReportService _reportService;


    public ReportsController(IBackgroundJobClient backgroundJobClient, ReportService reportService)
    {
        _backgroundJobClient = backgroundJobClient;
        _reportService = reportService;

    }

[HttpPost("trigger-monthly-report")]
public IActionResult TriggerMonthlyReport()
{
    // Enqueue the updated method
    var jobId = BackgroundJob.Enqueue<HangfireJobs>(
        x => x.GenerateAndSendMonthlyExpensesReport()
    );
    return Ok(new { jobId, message = "Report generation and email sending started" });
}

    [HttpGet("send-test-email")]
    public async Task<IActionResult> SendTestEmail()
    {
        await _reportService.SendTestEmail();
        return Ok("Email sent");
    }
}