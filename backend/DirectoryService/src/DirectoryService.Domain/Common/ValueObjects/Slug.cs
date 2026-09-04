using System.Text.RegularExpressions;
using ErrorOr;

namespace DirectoryService.Domain.Common.ValueObjects;

public sealed record Slug
{
    private static readonly Regex ValidSlug = new(@"^[a-z0-9-]{1,100}$", RegexOptions.Compiled);

    private Slug(string value) => Value = value;

    public string Value { get; }

    public static ErrorOr<Slug> Create(string value)
    {
        var normalized = value?.Trim().ToLowerInvariant() ?? string.Empty;

        if (!ValidSlug.IsMatch(normalized))
        {
            return Error.Validation("Slug.Invalid", "Slug must contain only lowercase letters, digits, and hyphens (1–100 characters)");
        }

        return new Slug(normalized);
    }
}
