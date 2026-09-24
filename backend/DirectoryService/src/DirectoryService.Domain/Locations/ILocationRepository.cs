namespace DirectoryService.Domain.Locations;

public interface ILocationRepository
{
    Task<bool> ExistsWithNameAsync(
        string name,
        CancellationToken cancellationToken = default);
    
    Task AddAsync(
        Location location,
        CancellationToken cancellationToken = default);
}
