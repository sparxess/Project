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
            new CommandDefinition(
                nameSelectSql,
                nameSelectParameters,
                cancellationToken: cancellationToken));
        
        return result;
    }

    public async Task<bool> AllExistAsync(
        IEnumerable<Guid> locationIds,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        var distinctIds = locationIds.Distinct().ToArray();

        const string locationExistCountSql = """
                           SELECT COUNT(*) FROM locations WHERE id = ANY(@Ids)
                           """;

        var locationExistCountParameters = new { Ids = distinctIds };
        
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                locationExistCountSql,
                locationExistCountParameters,
                cancellationToken: cancellationToken));

        return count == distinctIds.Length;
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
                new CommandDefinition(
                    locationAddSql,
                    locationAddParameters,
                    cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при сохранении локации {LocationId}", location.Id);
            throw;
        }
    }

    public async Task<Location?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
            
            var locationSelectSql = """
                                    SELECT id, name, address, created_at, updated_at
                                    FROM locations
                                    WHERE id = @Id
                                    """;
            
            var locationSelectParameters = new { Id = id };
            
            var locationRow = await connection.QuerySingleOrDefaultAsync<LocationRow>(
                new CommandDefinition(
                    locationSelectSql,
                    locationSelectParameters,
                    cancellationToken: cancellationToken));

            var result = locationRow is null
                ? null
                : Location.Restore(
                    locationRow.Id,
                    locationRow.Name,
                    locationRow.Address,
                    locationRow.CreatedAt,
                    locationRow.UpdatedAt);
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при пооиске локации {LocationId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(Location location, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
            
            var locationUpdateSql = """
                                    UPDATE locations
                                    SET name = @Name, address = @Address, updated_at = @UpdatedAt
                                    WHERE id = @Id
                                    """;

            var locationUpdateParamerets = new
            {
                Id = location.Id,
                Name = location.Name.Value,
                Address = location.Address.Value,
                UpdatedAt = location.UpdatedAt
            };
            
            await connection.ExecuteAsync(
                new CommandDefinition(
                    locationUpdateSql,
                    locationUpdateParamerets,
                    cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении локации {LocationId}", location.Id);
            throw;
        }
    }
    
    private sealed record LocationRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Address { get; init; } = null!;
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
    }
}
