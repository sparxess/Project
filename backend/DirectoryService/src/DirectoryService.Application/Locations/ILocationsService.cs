using DirectoryService.Contracts.Locations;

namespace DirectoryService.Application.Locations;

public interface ILocationsService
{
    Task<Guid> CreateAsync(
        CreateLocationDto locationDto,
        CancellationToken cancellationToken = default);
    
    Task UpdateAsync(
        Guid id,
        UpdateLocationDto locationDto,
        CancellationToken cancellationToken = default);
}
