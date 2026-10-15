using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentLocationNotFoundException : NotFoundException
{
    public DepartmentLocationNotFoundException(Guid id)
        : base(Errors.DepartmentErrors.LocationNotFound(id))
    {
    }
}