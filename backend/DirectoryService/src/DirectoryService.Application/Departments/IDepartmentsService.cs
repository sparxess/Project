using DirectoryService.Contracts.Departments;

namespace DirectoryService.Application.Departments;

public interface IDepartmentsService
{
    Task<Guid> CreateAsync(
        CreateDepartmentDto departmentDto,
        CancellationToken cancellationToken = default);
}