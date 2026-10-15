using DirectoryService.Application.Extensions;
using DirectoryService.Application.Locations.Fails.Exceptions;
using DirectoryService.Contracts.Locations;
using DirectoryService.Domain.Locations;
using DirectoryService.Shared;
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
        var validationResult = await createLocationValidator.ValidateAsync(locationDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new LocationValidationException(validationResult.ToErrors());
        }
        
        var nameExists = await repository.ExistsWithNameAsync(locationDto.Name, cancellationToken);
        if (nameExists)
        {
            throw new LocationNameExistsException(locationDto.Name);
        }

        var id = Guid.NewGuid();
        var result = Location.Create(id, locationDto.Name, BuildAddress(locationDto));
        if (result.IsError)
        {
            throw new LocationValidationException(
                [DomainError.Validation(
                    result.FirstError.Code,
                    result.FirstError.Description)]);
        }

        await repository.AddAsync(result.Value, cancellationToken);

        return id;
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateLocationDto locationDto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await updateLocationValidator.ValidateAsync(locationDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new LocationValidationException(validationResult.ToErrors());
        }

        var location = await repository.FindByIdAsync(id, cancellationToken);
        if (location == null)
        {
            throw new LocationNotFoundException(id);
        }

        var updatedLocation = location.Update(locationDto.Name, BuildAddress(locationDto));
        if (updatedLocation.IsError)
        {
            throw new LocationValidationException(
                [DomainError.Validation(
                    updatedLocation.FirstError.Code,
                    updatedLocation.FirstError.Description)]);
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
