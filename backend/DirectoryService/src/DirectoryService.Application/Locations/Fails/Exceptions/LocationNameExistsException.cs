using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Locations.Fails.Exceptions;

public class LocationNameExistsException : DomainException
{
    public LocationNameExistsException(string name)
        : base(Errors.LocationErrors.NameExists(name))
    {
    }
}