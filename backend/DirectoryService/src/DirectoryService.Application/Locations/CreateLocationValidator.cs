using DirectoryService.Contracts.Locations;
using FluentValidation;

namespace DirectoryService.Application.Locations;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithErrorCode("location.name.empty")
                .WithMessage("Наименование локации не может быть пустым.")
            .MaximumLength(200)
                .WithErrorCode("location.name.too_long")
                .WithMessage("Наименование локации не может превышать 200 символов.");

        RuleFor(x => x.City)
            .NotEmpty()
                .WithErrorCode("location.city.empty")
                .WithMessage("Город не может быть пустым.")
            .MaximumLength(100)
                .WithErrorCode("location.city.too_long")
                .WithMessage("Город не может превышать 100 символов.");

        RuleFor(x => x.Street)
            .NotEmpty()
                .WithErrorCode("location.street.empty")
                .WithMessage("Улица не может быть пустой.")
            .MaximumLength(200)
                .WithErrorCode("location.street.too_long")
                .WithMessage("Улица не может превышать 200 символов.");

        RuleFor(x => x.House)
            .NotEmpty()
                .WithErrorCode("location.house.empty")
                .WithMessage("Номер дома не может быть пустым.")
            .MaximumLength(20)
                .WithErrorCode("location.house.too_long")
                .WithMessage("Номер дома не может превышать 20 символов.");

        RuleFor(x => x.Apartment)
            .MaximumLength(20)
                .WithErrorCode("location.apartment.too_long")
                .WithMessage("Номер квартиры не может превышать 20 символов.")
            .When(x => x.Apartment is not null);
    }
}
