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

    Task UpdateAsync(
        Department department,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsDepartmentLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default);

    Task AddDepartmentLocationAsync(
        DepartmentLocation location,
        CancellationToken cancellationToken = default);

    Task<bool> RemoveDepartmentLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default);
}
