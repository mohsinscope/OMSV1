using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.DependencyInjection;
using OMSV1.Application.Handlers.DamagedDevices;
using OMSV1.Application.Handlers.DamagedPassports;
using OMSV1.Application.Handlers.Governorates;
using OMSV1.Application.Handlers.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Extensions;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddIdentityServices(builder.Configuration);
    // Add Identity
    // builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
    // {
    //     // Configure Identity options
    //     options.Password.RequireDigit = true;
    //     options.Password.RequiredLength = 8;
    //     options.Password.RequireNonAlphanumeric = true;
    //     options.User.RequireUniqueEmail = true;
    // })
    // .AddEntityFrameworkStores<AppDbContext>()
    // .AddDefaultTokenProviders();

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
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
