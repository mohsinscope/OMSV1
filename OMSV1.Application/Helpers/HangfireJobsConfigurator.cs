using Hangfire;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Configuration;

public static class HangfireJobsConfigurator
{
    public static void ConfigureRecurringJobs()
    {
        RecurringJob.AddOrUpdate<HangfireJobs>(
            "GenerateAndSendMonthlyReport",
            job => job.GenerateAndSendMonthlyExpensesReport(),
            Cron.Monthly(1),
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            }
        );
    }
}
