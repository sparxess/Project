using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using FluentValidation;

namespace DirectoryService.Application.Departments;

public class DepartmentsService(
    IDepartmentsRepository repository,
    ILocationsRepository locationsRepository,
    CreateDepartmentValidator createDepartmentValidator,
    UpdateDepartmentValidator updateDepartmentValidator) : IDepartmentsService
{
    public async Task<Guid> CreateAsync(
        CreateDepartmentDto departmentDto,
        CancellationToken cancellationToken = default)
    {
        await createDepartmentValidator.ValidateAndThrowAsync(departmentDto, cancellationToken);

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
        
        if (departmentDto.LocationIds.Count > 0)
        {
            var allExist = await locationsRepository.AllExistAsync(departmentDto.LocationIds, cancellationToken);
            if (!allExist)
            {
                throw new InvalidOperationException("Одна или несколько указанных локаций не существуют.");
            }
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

    public async Task UpdateAsync(
        Guid id,
        UpdateDepartmentDto departmentDto,
        CancellationToken cancellationToken = default)
    {
        await updateDepartmentValidator.ValidateAndThrowAsync(departmentDto, cancellationToken);
        
        var department = await repository.FindByIdAsync(id, cancellationToken);
        if (department == null)
        {
            throw new InvalidOperationException($"Подразделение с идентификатором {id} не найдено");
        }

        if (!string.Equals(departmentDto.Slug, department.Slug.Value, StringComparison.Ordinal))
        {
            var slugExists = await repository.ExistsWithSlugAsync(departmentDto.Slug, cancellationToken);
            if (slugExists)
            {
                throw new InvalidOperationException($"Slug '{departmentDto.Slug}' уже занят.");
            }
        }
        
        var updatedDepartment = department.Update(departmentDto.Name, departmentDto.Slug);
        if (updatedDepartment.IsError)
        {
            throw new InvalidOperationException(updatedDepartment.FirstError.Description);
        }
        
        await repository.UpdateAsync(department, cancellationToken);
    }

    public async Task AddLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        var department = await repository.FindByIdAsync(departmentId, cancellationToken);
        if (department == null)
        {
            throw new InvalidOperationException("Указанное подразделение не существует.");
        }
        
        var location = await locationsRepository.FindByIdAsync(locationId, cancellationToken);
        if (location == null)
        {
            throw new InvalidOperationException("Указанная локация не существует.");
        }
        
        var departmentLocationExists =
            await repository.ExistsDepartmentLocationAsync(departmentId, locationId, cancellationToken);
        if (departmentLocationExists)
        {
            throw new InvalidOperationException("Указанная локация уже привязана к данному подразделению.");
        }
        
        var id = Guid.NewGuid();
        var departmentLocation = DepartmentLocation.Create(id, departmentId, locationId);
        if (departmentLocation.IsError)
        {
            throw new InvalidOperationException(departmentLocation.FirstError.Description);
        }
        
        await repository.AddDepartmentLocationAsync(departmentLocation.Value, cancellationToken);
    }

    public async Task RemoveLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.RemoveDepartmentLocationAsync(departmentId, locationId, cancellationToken);
        if (!result)
        {
            throw new InvalidOperationException("Данная локация не привязана к указанному подразделению или не существует.");
        }
    }
}