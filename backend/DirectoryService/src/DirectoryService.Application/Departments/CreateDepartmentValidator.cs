using DirectoryService.Contracts.Departments;
using FluentValidation;

namespace DirectoryService.Application.Departments;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Наименование подразделения не может быть пустым.")
            .MaximumLength(200).WithMessage("Наименование подразделения не может превышать 200 символов.");
        
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug не может быть пустым.")
            .MaximumLength(100).WithMessage("Slug не может превышать 100 символов.");
        
        RuleForEach(x => x.LocationIds)
            .NotEqual(Guid.Empty).WithMessage("Идентификатор локации не может быть пустым.");
    }
}