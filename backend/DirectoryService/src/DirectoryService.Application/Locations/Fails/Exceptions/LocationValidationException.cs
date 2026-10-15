using DirectoryService.Application.Exceptions;
using DirectoryService.Shared;

namespace DirectoryService.Application.Locations.Fails.Exceptions;

public class LocationValidationException : ValidationException
{
    public LocationValidationException(DomainError[] errors)
        : base(errors)
    {
    }
}