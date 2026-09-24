using DirectoryService.Contracts.Locations;

namespace DirectoryService.Application.Locations;

public interface ILocationsService
{
    Task<Guid> CreateAsync(
        CreateLocationDto locationDto,
        CancellationToken cancellationToken = default);
}
