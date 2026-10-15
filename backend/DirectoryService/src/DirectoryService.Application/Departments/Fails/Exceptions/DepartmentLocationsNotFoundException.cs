using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentLocationsNotFoundException : NotFoundException
{
    public DepartmentLocationsNotFoundException()
        : base(Errors.DepartmentErrors.LocationsNotFound())
    {
    }
}