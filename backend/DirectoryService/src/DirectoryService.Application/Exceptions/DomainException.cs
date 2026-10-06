using DirectoryService.Shared;

namespace DirectoryService.Application.Exceptions;

public class DomainException : Exception
{
    public DomainError Error { get; }
    public DomainException(DomainError error) : base(error.Message) => Error = error;
}