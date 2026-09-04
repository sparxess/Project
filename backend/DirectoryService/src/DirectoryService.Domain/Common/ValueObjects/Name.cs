using ErrorOr;

namespace DirectoryService.Domain.Common.ValueObjects;

public sealed record Name
{
    private Name(string value) => Value = value;

    public string Value { get; }

    public static ErrorOr<Name> Create(string value)
    {
        var trimmedValue = value?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(trimmedValue))
        {
            return Error.Validation("Name.Empty", "Name cannot be empty");
        }

        return new Name(trimmedValue);
    }
}
