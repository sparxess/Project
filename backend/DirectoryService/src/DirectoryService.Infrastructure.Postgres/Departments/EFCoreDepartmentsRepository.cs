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
}