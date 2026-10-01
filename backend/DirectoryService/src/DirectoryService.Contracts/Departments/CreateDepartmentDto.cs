namespace DirectoryService.Contracts.Departments;

public record CreateDepartmentDto(
    string Name,
    string Slug,
    Guid? ParentId,
    IList<Guid> LocationIds
);