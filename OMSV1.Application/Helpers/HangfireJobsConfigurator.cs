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
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time")
            }
        );

        // Configure the daily damaged passports report job to run at 8:00 PM Baghdad time
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendDailyDamagedPassportsReport",
            job => job.GenerateAndSendDailyDamagedPassportsReport(),
            Cron.Daily(0, 1), // 00:01 in 24-hour format = 12:01 AM
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time")
            }
        );
        // Configure the daily damaged passports ZIP archive job to run at 8:00 PM Baghdad time
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendDailyDamagedPassportsZipArchiveReport",
            job => job.GenerateAndSendDailyDamagedPassportsZipArchiveReport(),
            Cron.Daily(0, 5), // 00:03 in 24-hour format = 12:03 AM
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time")
            }
        );


        // Configure the daily attendance report job to run at 8:05 PM Baghdad time
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendDailyAttendanceReport",
            job => job.GenerateAndSendDailyAttendanceReport(),
            Cron.Daily(0, 10), // 00:05 in 24-hour format = 12:05 AM
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time")
            }
        );

    }
}