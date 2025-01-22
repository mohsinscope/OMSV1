using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Infrastructure.Services;

namespace OMSV1.Infrastructure.Extensions;

public static class DocumentServicesExtensions
{
    public static IServiceCollection AddDocumentServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPdfService, ITextSharpPdfService>();
        return services;
    }
}