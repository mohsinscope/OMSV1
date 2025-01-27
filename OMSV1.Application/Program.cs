using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using OMSV1.Application.Authorization.Handlers;
using OMSV1.Application.Authorization.Providers;
using OMSV1.Application.Helpers;
using OMSV1.Application.Middleware;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Extensions;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Infrastructure.Repositories;
using OMSV1.Application.Queries.Expenses;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Handlers.Expenses;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using OMSV1.Infrastructure.Helpers;
using OMSV1.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add rate limiting services
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", config =>
    {
        config.PermitLimit = 50;
        config.Window = TimeSpan.FromSeconds(10);
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 20;
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.Headers.RetryAfter = "10";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many requests. Please try again later.",
            retryAfter = 10
        }, token);
    };
});

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "https://oms.scopesky.org",
                "http://oms.scopesky.org",
                "http://localhost:5173"
            )
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Content-Type", "Authorization")
            .AllowCredentials();
    });
});

// Configure Hangfire with PostgreSQL
// Update the Hangfire configuration to use the new method
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
    }));
builder.Services.AddHangfireServer();

// Register Hangfire-related services
builder.Services.AddScoped<HangfireJobs>();
//builder.Services.AddScoped<ScheduledPdfService>();

// Add MediatR, Identity, AutoMapper, and application services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicPermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();
builder.Services.AddDocumentServices(builder.Configuration);
builder.Services.AddScoped<MonthlyExpensesRepository, MonthlyExpensesRepository>();
//builder.Services.AddScoped<IRequestHandler<GetMonthlyExpensesQuery, string>, GetMonthlyExpensesQueryHandler>();
builder.Services.AddScoped<IMonthlyExpensesRepository, MonthlyExpensesRepository>();
builder.Services.AddScoped<IPdfService, ITextSharpPdfService>();
builder.Services.AddScoped<ITextSharpPdfService, ITextSharpPdfService>();

// Add services BEFORE Build()
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<ReportService>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
//Hangfire
builder.Services.AddSingleton<HangfireAuthorizationFilter>();


var app = builder.Build();

// Enable HTTPS redirection and HSTS
app.UseHttpsRedirection();
app.UseHsts();

// Apply CORS policy before authentication
app.UseCors("AllowSpecificOrigins");

// Add rate limiting middleware
app.UseRateLimiter();

// Add custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

// Apply authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure Hangfire dashboard
// Then in your app configuration:
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] 
    { 
        app.Services.GetRequiredService<HangfireAuthorizationFilter>() 
    }
});

HangfireJobsConfigurator.ConfigureRecurringJobs();
// Schedule recurring jobs
// RecurringJob.AddOrUpdate<HangfireJobs>(
//     "GenerateAndSendMonthlyReport", // Job ID
//     job => job.GenerateAndSendMonthlyExpensesReport(), // Method to execute
//     Cron.Monthly(1), // Runs on the 1st day of every month
//     new RecurringJobOptions
//     {
//         TimeZone = TimeZoneInfo.Utc // Adjust timezone if needed
//     }
// );


// Serve static files without caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        var headers = context.Context.Response.Headers;
        headers.Append("Access-Control-Allow-Origin", "*");
        headers.Append("X-Content-Type-Options", "nosniff");
    }
});

// Add Content Security Policy (CSP) header
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self'; img-src 'self' https://cdn-oms.scopesky.org; script-src 'self'; style-src 'self';");
    await next();
});

// Map controllers with rate limiting
app.MapControllers().RequireRateLimiting("api");

// Role and admin user initialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { "SuperAdmin", "Admin", "Supervisor", "Manager","I.T" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }

    var adminEmail = "admin";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(user, "Admin@123");
        await userManager.AddToRoleAsync(user, "SuperAdmin");
    }
}

app.Run();
