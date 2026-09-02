using DirectoryService.Domain.Common.ValueObjects;
using ErrorOr;

namespace DirectoryService.Domain.Locations;

public class Location
{
    private Location(Guid id, Name name, Address address)
    {
        Id = id;
        Name = name;
        Address = address;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    public Guid Id { get; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
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

        return new Location(id,  nameResult.Value, addressResult.Value);
    }
}