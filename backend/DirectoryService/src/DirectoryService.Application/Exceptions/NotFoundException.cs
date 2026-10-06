using DirectoryService.Shared;

namespace DirectoryService.Application.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(DomainError error) : base(error) {}
}