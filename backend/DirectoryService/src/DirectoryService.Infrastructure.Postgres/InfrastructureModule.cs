using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Infrastructure.Postgres.Departments;
using DirectoryService.Infrastructure.Postgres.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure.Postgres;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DirectoryService");

        services.AddDbContext<DirectoryServiceDbContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        services.AddNpgsqlDataSource(connectionString!);

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var repositoryImplementation = configuration["Infrastructure:LocationsRepository"];

        if (string.Equals(repositoryImplementation, "Dapper", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<ILocationsRepository, NpgsqlLocationsRepository>();
            services.AddScoped<IDepartmentsRepository, NpgsqlDepartmentsRepository>();
        }
        else
        {
            services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
            services.AddScoped<IDepartmentsRepository, EFCoreDepartmentsRepository>();
        }
        
        return services;
    }
}
