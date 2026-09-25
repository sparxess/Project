using Dapper;
using DirectoryService.Domain.Locations;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DirectoryService.Infrastructure.Postgres.Locations;

public class NpgsqlLocationsRepository(
    NpgsqlDataSource dataSource,
    ILogger<NpgsqlLocationsRepository> logger) : ILocationsRepository
{
    public async Task<bool> ExistsWithNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        const string nameSelectSql = """
                                     SELECT EXISTS(SELECT 1 FROM locations WHERE name = @Name)
                                     """;
        var nameSelectParameters = new
        {
            Name = name
        };

        var result = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(nameSelectSql, nameSelectParameters, cancellationToken: cancellationToken));
        
        return result;
    }

    public async Task AddAsync(
        Location location,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
        const string locationAddSql = """
                                      INSERT INTO locations (id, name, address, created_at, updated_at)
                                      VALUES (@Id, @Name, @Address, @CreatedAt, @UpdatedAt)
                                      """;

        var locationAddParameters = new
        {
            Id = location.Id,
            Name = location.Name.Value,
            Address = location.Address.Value,
            CreatedAt = location.CreatedAt,
            UpdatedAt = location.UpdatedAt
        };
        
        try
        {
            await connection.ExecuteAsync(
                new CommandDefinition(locationAddSql, locationAddParameters, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при сохранении локации {LocationId}", location.Id);
            throw;
        }
    }
}
