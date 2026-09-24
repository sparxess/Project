using DirectoryService.Application;

namespace DirectoryService.Api;

internal static class ApiModule
{
    public static IServiceCollection AddProgramServices(this IServiceCollection services)
    {
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
