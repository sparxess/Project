using DirectoryService.Domain.Common.ValueObjects;
using ErrorOr;

namespace DirectoryService.Domain.Positions;

public class Position
{
    private Position(Guid id, Name name)
    {
        Id = id;
        Name = name;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    public Guid Id { get; }
    public Name Name { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static ErrorOr<Position> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            return Error.Validation("Position.InvalidId", "Id cannot be empty");
        }

        var nameResult = Name.Create(name);
        if (nameResult.IsError)
        {
            return nameResult.Errors;
        }
        
        return new Position(id, nameResult.Value);
    }
}