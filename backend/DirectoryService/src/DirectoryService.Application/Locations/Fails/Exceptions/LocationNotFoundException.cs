using DirectoryService.Application.Exceptions;

namespace DirectoryService.Application.Locations.Fails.Exceptions;

public class LocationNotFoundException : NotFoundException
{
    public LocationNotFoundException(Guid id)
        : base(Errors.LocationErrors.NotFound(id))
    {
    }
}