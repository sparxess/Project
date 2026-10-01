using DirectoryService.Domain.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres.Departments;

public class EFCoreDepartmentsRepository(
    DirectoryServiceDbContext dbContext,
    ILogger<EFCoreDepartmentsRepository> logger) : IDepartmentsRepository
{
    public async Task<Department?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var department = await dbContext.Departments
            .Where(department => department.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return department;
    }

    public async Task<bool> ExistsWithSlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var result = await dbContext.Departments
            .AnyAsync(department => department.Slug.Value == slug, cancellationToken);
        
        return result;
    }

    public async Task AddAsync(
        Department department,
        IReadOnlyList<DepartmentLocation> locations,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Departments.AddAsync(department, cancellationToken);
            await dbContext.DepartmentLocations.AddRangeAsync(locations, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при сохранении локации {DepartmentId}", department.Id);
            throw;
        }
    }

    public async Task UpdateAsync(
        Department department,
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsDepartmentLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        var result = await dbContext.DepartmentLocations
            .AnyAsync(departmentLocation => departmentLocation.DepartmentId == departmentId
                                            && departmentLocation.LocationId == locationId,
                      cancellationToken);
        
        return result;
    }

    public async Task AddDepartmentLocationAsync(
        DepartmentLocation location,
        CancellationToken cancellationToken = default)
    {
        await dbContext.DepartmentLocations.AddAsync(location, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> RemoveDepartmentLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        var deletedCount = await dbContext.DepartmentLocations
            .Where(departmentLocation => departmentLocation.DepartmentId == departmentId
                                         && departmentLocation.LocationId == locationId)
            .ExecuteDeleteAsync(cancellationToken);
        
        return deletedCount > 0;
    }
}