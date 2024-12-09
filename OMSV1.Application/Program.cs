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

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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




app.Run();
