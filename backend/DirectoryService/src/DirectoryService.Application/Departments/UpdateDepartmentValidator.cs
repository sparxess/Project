using DirectoryService.Contracts.Departments;
using FluentValidation;

namespace DirectoryService.Application.Departments;

public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithErrorCode("department.name.empty")
                .WithMessage("Наименование подразделения не может быть пустым.")
            .MaximumLength(200)
                .WithErrorCode("department.name.too_long")
                .WithMessage("Наименование подразделения не может превышать 200 символов.");

        RuleFor(x => x.Slug)
            .NotEmpty()
                .WithErrorCode("department.slug.empty")
                .WithMessage("Slug не может быть пустым.")
            .MaximumLength(100)
                .WithErrorCode("department.slug.too_long")
                .WithMessage("Slug не может превышать 100 символов.");
    }
}