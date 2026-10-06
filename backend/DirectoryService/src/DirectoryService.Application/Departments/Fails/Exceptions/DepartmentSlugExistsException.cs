using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentSlugExistsException : DomainException
{
    public DepartmentSlugExistsException(string slug)
        : base(Errors.DepartmentErrors.SlugExists(slug))
    {
    }
}