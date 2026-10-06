using DirectoryService.Application.Exceptions;
using DirectoryService.Shared;

namespace DirectoryService.Application.Departments.Fails.Exceptions;

public class DepartmentValidationException : ValidationException
{
    public DepartmentValidationException(DomainError[] errors)
        : base(errors)
    {
    }
}