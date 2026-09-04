namespace DirectoryService.Domain.Common.ValueObjects;

public sealed record DepartmentPath
{
    private DepartmentPath(string value) => Value = value;

    public string Value { get; }

    public static DepartmentPath CreateForRoot(Slug slug) =>
        new(slug.Value);

    public static DepartmentPath Append(DepartmentPath parent, Slug slug) =>
        new($"{parent.Value}/{slug.Value}");

    internal static DepartmentPath FromRaw(string value) => new(value);
}
