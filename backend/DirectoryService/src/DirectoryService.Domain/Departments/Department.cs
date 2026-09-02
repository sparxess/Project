using DirectoryService.Domain.Common.ValueObjects;
using ErrorOr;

namespace DirectoryService.Domain.Departments;

public class Department
{
    private Department(Guid id, Name name, Slug slug, DepartmentPath path, Guid? parentId)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Path = path;
        ParentId = parentId;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; }
    public Name Name { get; private set; }
    public Slug Slug { get; private set; }
    public DepartmentPath Path { get; private set; }
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
            return Error.Validation("Department.InvalidId", "Id cannot be empty");
        }
        
        if (parentId == Guid.Empty)
        {
            return Error.Validation("Department.InvalidParentId", "ParentId cannot be an empty Guid");
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

        return new Department(id, nameResult.Value, slugResult.Value, path, parentId);
    }
}
