using DirectoryService.Contracts.Locations;
using FluentValidation;

namespace DirectoryService.Application.Locations;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Наименование локации не может быть пустым.")
            .MaximumLength(200).WithMessage("Наименование локации не может превышать 200 символов.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Город не может быть пустым.")
            .MaximumLength(100).WithMessage("Город не может превышать 100 символов.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Улица не может быть пустой.")
            .MaximumLength(200).WithMessage("Улица не может превышать 200 символов.");

        RuleFor(x => x.House)
            .NotEmpty().WithMessage("Номер дома не может быть пустым.")
            .MaximumLength(20).WithMessage("Номер дома не может превышать 20 символов.");

        RuleFor(x => x.Apartment)
            .MaximumLength(20).WithMessage("Номер квартиры не может превышать 20 символов.")
            .When(x => x.Apartment is not null);
    }
}
