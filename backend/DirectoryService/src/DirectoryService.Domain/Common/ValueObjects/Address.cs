using ErrorOr;

namespace DirectoryService.Domain.Common.ValueObjects;

public sealed record Address
{
    private Address(string value) => Value = value;
    
    public string Value { get; }

    public static ErrorOr<Address> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation("Address.Empty",  "Address cannot be empty");
        }

        return new Address(value);
    }
}