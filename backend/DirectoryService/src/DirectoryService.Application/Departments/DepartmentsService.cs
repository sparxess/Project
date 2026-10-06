using DirectoryService.Application.Departments.Fails.Exceptions;
using DirectoryService.Application.Extensions;
using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Shared;
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
        var validationResult = await createDepartmentValidator.ValidateAsync(departmentDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new DepartmentValidationException(validationResult.ToErrors());
        }

        string? parentPath = null;
        if (departmentDto.ParentId != null)
        {
            var parentDepartment = await repository.FindByIdAsync(departmentDto.ParentId.Value, cancellationToken);
            if (parentDepartment == null)
            {
                throw new DepartmentParentNotFoundException(departmentDto.ParentId!.Value);
            }

            parentPath = parentDepartment.Path.Value;
        }
        
        var slugExists = await repository.ExistsWithSlugAsync(departmentDto.Slug, cancellationToken);
        if (slugExists)
        {
            throw new DepartmentSlugExistsException(departmentDto.Slug);
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
            throw new DepartmentValidationException(
                [DomainError.Validation(
                    department.FirstError.Code,
                    department.FirstError.Description)]);
        }
        
        if (departmentDto.LocationIds.Count > 0)
        {
            var allExist = await locationsRepository.AllExistAsync(departmentDto.LocationIds, cancellationToken);
            if (!allExist)
            {
                throw new DepartmentLocationsNotFoundException();
            }
        }

        var locations = departmentDto.LocationIds.Select(locationId =>
        {
            var id = Guid.NewGuid();
            var location = DepartmentLocation.Create(id, department.Value.Id, locationId);
            if (location.IsError)
            {
                throw new DepartmentValidationException(
                    [DomainError.Validation(
                        location.FirstError.Code,
                        location.FirstError.Description)]);
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
        var validationResult = await updateDepartmentValidator.ValidateAsync(departmentDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new DepartmentValidationException(validationResult.ToErrors());
        }
        
        var department = await repository.FindByIdAsync(id, cancellationToken);
        if (department == null)
        {
            throw new DepartmentNotFoundException(id);
        }

        if (!string.Equals(departmentDto.Slug, department.Slug.Value, StringComparison.Ordinal))
        {
            var slugExists = await repository.ExistsWithSlugAsync(departmentDto.Slug, cancellationToken);
            if (slugExists)
            {
                throw new DepartmentSlugExistsException(departmentDto.Slug);
            }
        }
        
        var updatedDepartment = department.Update(departmentDto.Name, departmentDto.Slug);
        if (updatedDepartment.IsError)
        {
            throw new DepartmentValidationException(
                [DomainError.Validation(
                    updatedDepartment.FirstError.Code,
                    updatedDepartment.FirstError.Description)]);
            
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
            throw new DepartmentNotFoundException(departmentId);
        }
        
        var location = await locationsRepository.FindByIdAsync(locationId, cancellationToken);
        if (location == null)
        {
            throw new DepartmentLocationNotFoundException(locationId);
        }
        
        var departmentLocationExists =
            await repository.ExistsDepartmentLocationAsync(departmentId, locationId, cancellationToken);
        if (departmentLocationExists)
        {
            throw new DepartmentLocationAlreadyExistsException(locationId);
        }
        
        var id = Guid.NewGuid();
        var departmentLocation = DepartmentLocation.Create(id, departmentId, locationId);
        if (departmentLocation.IsError)
        {
            throw new DepartmentValidationException(
                [DomainError.Validation(
                    departmentLocation.FirstError.Code,
                    departmentLocation.FirstError.Description)]);
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
            throw new DepartmentLocationNotLinkedException(locationId);
        }
    }
}
