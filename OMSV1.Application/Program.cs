using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Helpers;
using OMSV1.Application.Middleware;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Extensions;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddIdentityServices(builder.Configuration);



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();






builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();



app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://https://oms.scopesky.org","https://https://oms.scopesky.org"));

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigins", policy =>
//     {
//         policy.WithOrigins("https://oms.scopesky.org")
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials();
//     });
// });

app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Create roles
    var roles = new[] { "Admin", "Manager", "Supervisor" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }

    // Create an admin user
    var adminEmail = "admin";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(user, "Admin@123");
        await userManager.AddToRoleAsync(user, "Admin");
    }
}



app.Run();
