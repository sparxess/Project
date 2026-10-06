using DirectoryService.Shared;

namespace DirectoryService.Application.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyList<DomainError> Errors { get; }
    public ValidationException(DomainError[] errors) : base("Validation failed") => Errors = errors;
}