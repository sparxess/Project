using DirectoryService.Application;
using DirectoryService.Infrastructure.Postgres;

namespace DirectoryService.Api;

internal static class ApiModule
{
    public static IServiceCollection AddProgramServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddApplicationServices();
        services.AddApiServices();
        return services;
    }

    private static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddHealthChecks();

        return services;
    }
}
