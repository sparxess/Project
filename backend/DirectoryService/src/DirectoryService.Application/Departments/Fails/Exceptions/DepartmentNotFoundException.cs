using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentNotFoundException : NotFoundException
{
    public DepartmentNotFoundException(Guid id)
        : base(Errors.DepartmentErrors.NotFound(id))
    {
    }
}