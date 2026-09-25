using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres.Locations;

public class EfCoreLocationsRepository(
    DirectoryServiceDbContext dbContext,
    ILogger<EfCoreLocationsRepository> logger) : ILocationsRepository
{
    public async Task<bool> ExistsWithNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var nameExists = await dbContext.Locations
            .AnyAsync(location => location.Name.Value == name, cancellationToken);

        return nameExists;
    }

    public async Task AddAsync(
        Location location,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Locations.AddAsync(location, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при сохранении локации {LocationId}", location.Id);
            throw;
        }
    }
}