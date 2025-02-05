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
            Cron.Daily(20, 0), // 20:00 in 24-hour format = 8:00 PM
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time")
            }
        );
        // Configure the daily damaged passports ZIP archive job to run at 8:00 PM Baghdad time
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendDailyDamagedPassportsZipArchiveReport",
            job => job.GenerateAndSendDailyDamagedPassportsZipArchiveReport(),
            Cron.Daily(20, 0),
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time")
            }
        );


        // Configure the daily attendance report job to run at 8:05 PM Baghdad time
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendDailyAttendanceReport",
            job => job.GenerateAndSendDailyAttendanceReport(),
            Cron.Daily(20, 5), // 20:05 in 24-hour format = 8:05 PM
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time")
            }
        );

    }
}