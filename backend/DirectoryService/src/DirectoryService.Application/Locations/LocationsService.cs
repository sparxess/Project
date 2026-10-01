using DirectoryService.Contracts.Locations;
using DirectoryService.Domain.Locations;
using FluentValidation;

namespace DirectoryService.Application.Locations;

public class LocationsService(
    ILocationsRepository repository,
    CreateLocationValidator createLocationValidator,
    UpdateLocationValidator updateLocationValidator) : ILocationsService
{
    public async Task<Guid> CreateAsync(
        CreateLocationDto locationDto,
        CancellationToken cancellationToken = default)
    {
        await createLocationValidator.ValidateAndThrowAsync(locationDto, cancellationToken);

        var nameExists = await repository.ExistsWithNameAsync(locationDto.Name, cancellationToken);
        if (nameExists)
        {
            throw new InvalidOperationException($"Локация с именем '{locationDto.Name}' уже существует.");
        }

        var id = Guid.NewGuid();
        var result = Location.Create(id, locationDto.Name, BuildAddress(locationDto));
        if (result.IsError)
        {
            throw new InvalidOperationException(result.FirstError.Description);
        }

        await repository.AddAsync(result.Value, cancellationToken);

        return id;
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateLocationDto locationDto,
        CancellationToken cancellationToken = default)
    {
        await updateLocationValidator.ValidateAndThrowAsync(locationDto, cancellationToken);

        var location = await repository.FindByIdAsync(id, cancellationToken);
        if (location == null)
        {
            throw new InvalidOperationException($"Локация с идентификатором {id} не найдена.");
        }

        var updatedLocation = location.Update(locationDto.Name, BuildAddress(locationDto));
        if (updatedLocation.IsError)
        {
            throw new InvalidOperationException(updatedLocation.FirstError.Description);
        }

        await repository.UpdateAsync(location, cancellationToken);
    }

    private static string BuildAddress(CreateLocationDto location) =>
        BuildAddress(location.City, location.Street, location.House, location.Apartment);

    private static string BuildAddress(UpdateLocationDto location) =>
        BuildAddress(location.City, location.Street, location.House, location.Apartment);

    private static string BuildAddress(string city, string street, string house, string? apartment) =>
        string.Join(", ", new[] { city, street, house, apartment }
            .Where(addressPart => !string.IsNullOrWhiteSpace(addressPart)));
}
