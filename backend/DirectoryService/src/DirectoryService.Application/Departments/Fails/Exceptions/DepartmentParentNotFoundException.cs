using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentParentNotFoundException : NotFoundException
{
    public DepartmentParentNotFoundException(Guid id)
        : base(Errors.DepartmentErrors.ParentNotFound(id))
    {
    }
}