using Hangfire;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Configuration;

public static class HangfireJobsConfigurator
{
    public static void ConfigureRecurringJobs()
    {
        // Configure the monthly expenses report job
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendMonthlyReport",
            job => job.GenerateAndSendMonthlyExpensesReport(),
            Cron.Monthly(1), // Run on the 1st day of every month
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            }
        );

        // Configure the daily damaged passports report job
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendDailyDamagedPassportsReport",
            job => job.GenerateAndSendDailyDamagedPassportsReport(),
            Cron.Daily(0, 0), // Run daily at midnight (00:00 UTC)
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            }
        );
              // ✅ Configure the daily attendance report job
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendDailyAttendanceReport",
            job => job.GenerateAndSendDailyAttendanceReport(),
            Cron.Daily(0, 5), // Runs daily at 00:05 UTC to avoid overlap
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            }
        );
    }
}