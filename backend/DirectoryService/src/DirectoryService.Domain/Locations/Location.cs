using DirectoryService.Domain.Common.ValueObjects;
using ErrorOr;

namespace DirectoryService.Domain.Locations;

public class Location
{
    // EF Core
    private Location() { }
    
    private Location(Guid id, Name name, Address address, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        Address = address;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    public Guid Id { get; }
    public Name Name { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static ErrorOr<Location> Create(Guid id, string name, string address)
    {
        if (id == Guid.Empty)
        {
            return Error.Validation("Location.InvalidId", "Id cannot be empty");
        }
        
        var nameResult = Name.Create(name);
        if (nameResult.IsError)
        {
            return nameResult.Errors;
        }
        
        var addressResult = Address.Create(address);
        if (addressResult.IsError)
        {
            return addressResult.Errors;
        }

        var now = DateTimeOffset.UtcNow;
        return new Location(id,  nameResult.Value, addressResult.Value, now, now);
    }

    public ErrorOr<Updated> Update(string name, string address)
    {
        var nameResult = Name.Create(name);
        if (nameResult.IsError)
        {
            return nameResult.Errors;
        }
        
        var addressResult = Address.Create(address);
        if (addressResult.IsError)
        {
            return addressResult.Errors;
        }
        
        Name = nameResult.Value;
        Address = addressResult.Value;
        UpdatedAt = DateTimeOffset.UtcNow;
        
        return Result.Updated;
    }

    public static Location Restore(
        Guid id,
        string name,
        string address,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        return new Location(
            id,
            Name.Create(name).Value,
            Address.Create(address).Value,
            createdAt,
            updatedAt);
    }
}