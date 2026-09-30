using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Departments;
using FluentValidation;

namespace DirectoryService.Application.Departments;

public class DepartmentsService(
    IDepartmentsRepository repository,
    CreateDepartmentValidator validator) : IDepartmentsService
{
    public async Task<Guid> CreateAsync(
        CreateDepartmentDto departmentDto,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(departmentDto, cancellationToken);

        string? parentPath = null;
        if (departmentDto.ParentId != null)
        {
            var parentDepartment = await repository.FindByIdAsync(departmentDto.ParentId.Value, cancellationToken);
            if (parentDepartment == null)
            {
                throw new InvalidOperationException($"Указанное родительское подразделение '{departmentDto.ParentId}' не найдено.");
            }

            parentPath = parentDepartment.Path.Value;
        }
        
        var slugExists = await repository.ExistsWithSlugAsync(departmentDto.Slug, cancellationToken);
        if (slugExists)
        {
            throw new InvalidOperationException($"Slug '{departmentDto.Slug}' уже занят.");
        }
        
        var departmentId = Guid.NewGuid();
        var department = Department.Create(
            departmentId,
            departmentDto.Name,
            departmentDto.Slug,
            departmentDto.ParentId,
            parentPath);

        if (department.IsError)
        {
            throw new InvalidOperationException(department.FirstError.Description);
        }
        
        var locations = departmentDto.LocationIds.Select(locationId =>
        {
            var id = Guid.NewGuid();
            var location = DepartmentLocation.Create(id, department.Value.Id, locationId);
            if (location.IsError)
            {
                throw new InvalidOperationException(location.FirstError.Description);
            }
            
            return location.Value;
        }).ToList();

        await repository.AddAsync(department.Value, locations, cancellationToken);

        return departmentId;
    }
}