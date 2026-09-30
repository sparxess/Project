namespace DirectoryService.Domain.Departments;

public interface IDepartmentsRepository
{
    Task<Department?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<bool> ExistsWithSlugAsync(
        string slug,
        CancellationToken cancellationToken = default);
 
    Task AddAsync(
        Department department,
        IReadOnlyList<DepartmentLocation> locations,
        CancellationToken cancellationToken = default);
}