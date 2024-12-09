using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Handlers.DamagedDevices;
using OMSV1.Application.Handlers.DamagedPassports;
using OMSV1.Application.Handlers.Governorates;
using OMSV1.Application.Handlers.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Extensions;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddIdentityServices(builder.Configuration);
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Add Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// Register MediatR
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllOfficesQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllGovernoratesQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllDamagedPassportsQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllDamagedDevicesQueryHandler).Assembly));

// Use Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Autofac Module Registration
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register Role-Based Modules
    containerBuilder.RegisterModule(new AdminRoleModule());
    containerBuilder.RegisterModule(new SupervisorRoleModule());
    //containerBuilder.RegisterModule(new EmployeeRoleModule());
    containerBuilder.RegisterModule(new ManagerRoleModule());
    //containerBuilder.RegisterModule(new EmployeeOfExpensesRoleModule());
    //containerBuilder.RegisterModule(new EmployeeOfDamagedRoleModule());
});
// Add Controllers
// this is just a temporary method before we implement DTO into our system
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var app = builder.Build();

app.UseHttpsRedirection();
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
    var adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(user, "Admin@123");
        await userManager.AddToRoleAsync(user, "Admin");
    }
}



app.Run();
