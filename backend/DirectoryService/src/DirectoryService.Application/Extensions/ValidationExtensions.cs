using DirectoryService.Shared;
using FluentValidation.Results;

namespace DirectoryService.Application.Extensions;

public static class ValidationExtensions
{
    public static DomainError[] ToErrors(this ValidationResult validationResult) =>
        validationResult.Errors.Select(failure => DomainError.Validation(
            failure.ErrorCode, failure.ErrorMessage, failure.PropertyName)).ToArray();
}