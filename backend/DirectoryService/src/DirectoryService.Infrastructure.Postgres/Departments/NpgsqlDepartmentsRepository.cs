using Dapper;
using DirectoryService.Domain.Departments;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DirectoryService.Infrastructure.Postgres.Departments;

public class NpgsqlDepartmentsRepository(
    NpgsqlDataSource dataSource,
    ILogger<NpgsqlDepartmentsRepository> logger) : IDepartmentsRepository
{
    public async Task<Department?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

            var departmentSelectSql = """
                                      SELECT id, name, slug, path, parent_id, created_at, updated_at
                                      FROM departments
                                      WHERE id = @Id
                                      """;

            var departmentSelectParameters = new { Id = id };

            var departmentRow = await connection.QuerySingleOrDefaultAsync<DepartmentRow>(
                new CommandDefinition(
                    departmentSelectSql,
                    departmentSelectParameters,
                    cancellationToken: cancellationToken));

            var result = departmentRow is null
                ? null
                : Department.Restore(
                    departmentRow.Id,
                    departmentRow.Name,
                    departmentRow.Slug,
                    departmentRow.Path,
                    departmentRow.ParentId,
                    departmentRow.CreatedAt,
                    departmentRow.UpdatedAt);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении подразделения по Id {DepartmentId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsWithSlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

            var slugSelectSql = """
                                SELECT EXISTS(SELECT 1 FROM departments WHERE slug = @Slug)
                                """;

            var slugSelectParameters = new { Slug = slug };

            var result = await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    slugSelectSql,
                    slugSelectParameters,
                    cancellationToken: cancellationToken));

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при проверке занятости slug '{Slug}'", slug);
            throw;
        }
    }

    public async Task AddAsync(
        Department department,
        IReadOnlyList<DepartmentLocation> locations,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var departmentAddSql = """
                                   INSERT INTO departments (id, name, slug, path, parent_id, created_at, updated_at)
                                   VALUES (@Id, @Name, @Slug, @Path, @ParentId, @CreatedAt, @UpdatedAt)
                                   """;

            var departmentAddParameters = new
            {
                Id = department.Id,
                Name = department.Name.Value,
                Slug = department.Slug.Value,
                Path = department.Path.Value,
                ParentId = department.ParentId,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt
            };

            await connection.ExecuteAsync(
                new CommandDefinition(
                    departmentAddSql,
                    departmentAddParameters,
                    transaction,
                    cancellationToken: cancellationToken));

            if (locations.Count > 0)
            {
                var locationAddSql = """
                                     INSERT INTO department_locations (id, department_id, location_id, is_primary)
                                     VALUES (@Id, @DepartmentId, @LocationId, @IsPrimary)
                                     """;

                var locationParameters = locations.Select(location => new
                {
                    Id = location.Id,
                    DepartmentId = location.DepartmentId,
                    LocationId = location.LocationId,
                    IsPrimary = location.IsPrimary
                });

                await connection.ExecuteAsync(
                    new CommandDefinition(
                        locationAddSql,
                        locationParameters,
                        transaction,
                        cancellationToken: cancellationToken));
            }


            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при сохранении подразделения {DepartmentId}", department.Id);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task UpdateAsync(
        Department department,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
            var departmentUpdateSql = """
                                      UPDATE departments
                                      SET name = @Name, slug = @Slug, updated_at = @UpdatedAt
                                      WHERE id = @Id
                                      """;

            var departmentUpdateParameters = new
            {
                Id = department.Id,
                Name = department.Name.Value,
                Slug = department.Slug.Value,
                UpdatedAt = department.UpdatedAt
            };
        
            await connection.ExecuteAsync(
                new CommandDefinition(
                    departmentUpdateSql,
                    departmentUpdateParameters,
                    cancellationToken: cancellationToken));
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении подразделения {DepartmentId}", department.Id);
            throw;
        }
    }

    public async Task<bool> ExistsDepartmentLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
            var locationExistsSql = """
                                    SELECT EXISTS(SELECT 1 FROM department_locations 
                                                  WHERE department_id = @DepartmentId AND location_id = @LocationId)
                                    """;

            var locationExistsParameters = new
            {
                DepartmentId = departmentId,
                LocationId = locationId
            };
        
            var result = await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    locationExistsSql, 
                    locationExistsParameters,
                    cancellationToken: cancellationToken));

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при проверке привязки локации {LocationId} к подразделению {DepartmentId}",  locationId, departmentId);
            throw;
        }
    }

    public async Task AddDepartmentLocationAsync(
        DepartmentLocation location,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
            var locationAddSql = """
                                 INSERT INTO department_locations (id, department_id, location_id, is_primary)
                                 VALUES (@Id, @DepartmentId, @LocationId, @IsPrimary)
                                 """;

            var locationAddParameters = new
            {
                Id = location.Id,
                DepartmentId = location.DepartmentId,
                LocationId = location.LocationId,
                IsPrimary = location.IsPrimary
            };
            
            await connection.ExecuteAsync(
                new CommandDefinition(
                    locationAddSql,
                    locationAddParameters,
                    cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при привязке локации {LocationId} к подразделению {DepartmentId}",  location.LocationId, location.DepartmentId);
            throw;
        }
    }

    public async Task<bool> RemoveDepartmentLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
            var locationDeleteSql = """
                                    DELETE FROM department_locations
                                    WHERE department_id = @DepartmentId AND  location_id = @LocationId
                                    """;

            var locationDeleteParameters = new
            {
                DepartmentId = departmentId,
                LocationId = locationId
            };
            
            var deletedCount = await connection.ExecuteAsync(
                new CommandDefinition(
                    locationDeleteSql,
                    locationDeleteParameters,
                    cancellationToken: cancellationToken));
            
            return deletedCount > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при удалении локации {LocationId} у подразделения {DepartmentId}", locationId, departmentId);
            throw;
        }
    }

    private sealed record DepartmentRow
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public string Path { get; init; } = null!;
        public Guid? ParentId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
    }
}