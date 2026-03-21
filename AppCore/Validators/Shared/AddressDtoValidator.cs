using AppCore.Dto;
using FluentValidation;

namespace AppCore.Validators.Shared;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Ulica nie może być pusta")
            .MaximumLength(200).WithMessage("Ulica nie może być dłuższa niż 200 znaków");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Miasto nie może być puste")
            .MaximumLength(100).WithMessage("Miasto nie może być dłuższe niż 100 znaków");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Kod pocztowy nie może być pusty")
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Kod pocztowy musi być w formacie xx-xxx");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Kraj nie może być pusty")
            .MaximumLength(100).WithMessage("Kraj nie może być dłuższy niż 100 znaków");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Typ adresu musi być poprawną wartością");
    }
}