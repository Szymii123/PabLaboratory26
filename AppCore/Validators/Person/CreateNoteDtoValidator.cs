using AppCore.Dto;
using FluentValidation;

namespace AppCore.Validators.Person;

public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
{
    public CreateNoteDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Treść notatki jest wymagana")
            .MaximumLength(200).WithMessage("Długość treści notatki nie może przekraczać 200 znaków.");
    }
}