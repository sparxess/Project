using DirectoryService.Contracts.Departments;

namespace DirectoryService.Application.Departments;

public interface IDepartmentsService
{
    Task<Guid> CreateAsync(
        CreateDepartmentDto departmentDto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Guid id,
        UpdateDepartmentDto departmentDto,
        CancellationToken cancellationToken = default);
    
    Task AddLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default);
    
    Task RemoveLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default);
}