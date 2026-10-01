namespace DirectoryService.Domain.Locations;

public interface ILocationsRepository
{
    Task<bool> ExistsWithNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<bool> AllExistAsync(
        IEnumerable<Guid> locationIds,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Location location,
        CancellationToken cancellationToken = default);

    Task<Location?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Location location,
        CancellationToken cancellationToken = default);
}
