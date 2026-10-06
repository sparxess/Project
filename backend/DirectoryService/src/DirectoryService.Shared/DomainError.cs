using System.Text.Json.Serialization;

namespace DirectoryService.Shared;

public record DomainError
{
    public string Code { get; }
    public string Message { get; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ErrorType Type { get; }
    public string? InvalidField { get; }

    private DomainError(
        string code,
        string message,
        ErrorType type,
        string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }
    
    public static DomainError NotFound(string? code, string message, Guid? id)
        => new(code ?? "record.not.found", message, ErrorType.NotFound);
    
    public static DomainError Validation(string? code, string message, string? invalidField = null)
        => new(code ?? "value.is.invalid", message, ErrorType.Validation, invalidField);
    
    public static DomainError Conflict(string? code, string message)
        => new(code ?? "value.is.conflict", message, ErrorType.Conflict);
    
    public static DomainError Failure(string? code, string message)
        => new(code ?? "failure", message, ErrorType.Failure);
}