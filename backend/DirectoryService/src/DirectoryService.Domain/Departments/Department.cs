using DirectoryService.Domain.Common.ValueObjects;
using ErrorOr;

namespace DirectoryService.Domain.Departments;

public class Department
{
    // EF Core
    private Department() { }

    private Department(
        Guid id,
        Name name,
        Slug slug,
        DepartmentPath path,
        Guid? parentId,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Path = path;
        ParentId = parentId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }
    public Name Name { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public DepartmentPath Path { get; private set; } = null!;
    public Guid? ParentId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static ErrorOr<Department> Create(
        Guid id,
        string name,
        string slug,
        Guid? parentId = null,
        string? parentPath = null)
    {
        if (id == Guid.Empty)
        {
            return Error.Validation(
                "department.invalid_id",
                "Id не может быть пустым.");
        }

        if (parentId == Guid.Empty)
        {
            return Error.Validation(
                "department.invalid_parent_id",
                "ParentId не может быть пустым.");
        }

        var nameResult = Name.Create(name);
        if (nameResult.IsError)
        {
            return nameResult.Errors;
        }

        var slugResult = Slug.Create(slug);
        if (slugResult.IsError)
        {
            return slugResult.Errors;
        }

        var path = parentPath is null
            ? DepartmentPath.CreateForRoot(slugResult.Value)
            : DepartmentPath.Append(DepartmentPath.FromRaw(parentPath), slugResult.Value);

        var now = DateTimeOffset.UtcNow;
        return new Department(id, nameResult.Value, slugResult.Value, path, parentId, now, now);
    }

    public static Department Restore(
        Guid id,
        string name,
        string slug,
        string path,
        Guid? parentId,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        return new Department(
            id,
            Name.Create(name).Value,
            Slug.Create(slug).Value,
            DepartmentPath.FromRaw(path),
            parentId,
            createdAt,
            updatedAt);
    }

    public ErrorOr<Updated> Update(string name, string slug)
    {
        var nameResult = Name.Create(name);
        if (nameResult.IsError)
        {
            return nameResult.Errors;
        }

        var slugResult = Slug.Create(slug);
        if (slugResult.IsError)
        {
            return slugResult.Errors;
        }

        Name = nameResult.Value;
        Slug = slugResult.Value;
        UpdatedAt = DateTimeOffset.UtcNow;
        
        return Result.Updated;
    }
}
