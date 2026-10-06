using DirectoryService.Shared;

namespace DirectoryService.Application.Exceptions;

public class BadRequestException : DomainException
{
    public BadRequestException(DomainError error) : base(error) {}
}