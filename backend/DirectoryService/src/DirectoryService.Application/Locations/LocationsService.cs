using DirectoryService.Contracts.Locations;
using DirectoryService.Domain.Locations;
using FluentValidation;

namespace DirectoryService.Application.Locations;

public class LocationsService(
    ILocationRepository repository,
    CreateLocationValidator validator) : ILocationsService
{
    public async Task<Guid> CreateAsync(
        CreateLocationDto locationDto,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(locationDto, cancellationToken);

        var nameExists = await repository.ExistsWithNameAsync(locationDto.Name, cancellationToken);
        if (nameExists)
        {
            throw new InvalidOperationException($"Локация с именем '{locationDto.Name}' уже существует.");
        }

        var address = string.Join(", ", new[]
            {
                locationDto.City,
                locationDto.Street,
                locationDto.House,
                locationDto.Apartment
            }
            .Where(addressParameter => !string.IsNullOrWhiteSpace(addressParameter)));

        var id = Guid.NewGuid();
        var result = Location.Create(id, locationDto.Name, address);
        if (result.IsError)
        {
            throw new InvalidOperationException(result.FirstError.Description);
        }

        await repository.AddAsync(result.Value, cancellationToken);

        return id;
    }
}
