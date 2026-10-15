using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentLocationAlreadyExistsException : DomainException
{
    public DepartmentLocationAlreadyExistsException(Guid id)
        : base(Errors.DepartmentErrors.LocationAlreadyExists(id))
    {
    }
}