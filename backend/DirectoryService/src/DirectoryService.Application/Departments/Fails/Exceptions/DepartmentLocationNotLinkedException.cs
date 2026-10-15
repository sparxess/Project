using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentLocationNotLinkedException : NotFoundException
{
    public DepartmentLocationNotLinkedException(Guid id)
        : base(Errors.DepartmentErrors.LocationNotLinked(id))
    {
    }
}