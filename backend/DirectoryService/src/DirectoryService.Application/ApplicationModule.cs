using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ApplicationModule).Assembly);

        services.AddScoped<ILocationsService, LocationsService>();
        services.AddScoped<IDepartmentsService, DepartmentsService>();

        return services;
    }
}
