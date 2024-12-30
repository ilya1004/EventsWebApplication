using EventsWebApplication.API.Contracts.Events;
using FluentValidation;

namespace EventsWebApplication.API.Validators.Events;

public class AddParticipantToEventRequestValidator : AbstractValidator<AddParticipantToEventRequest>
{
    public AddParticipantToEventRequestValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EventId is required.")
            .GreaterThanOrEqualTo(1).WithMessage("EventId cannot be less than 1");

        RuleFor(x => x.UserInfoDTO.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not be longer than 200 characters.");

        RuleFor(x => x.UserInfoDTO.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .MaximumLength(200).WithMessage("Surname must not be longer than 200 characters.");

        RuleFor(x => x.UserInfoDTO.Birthday)
            .NotEmpty().WithMessage("Birthday is required.")
            .Must(IsAgeValid).WithMessage("You must be at least 18 years old.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Birthday cannot be in the future.");
    }

    private bool IsAgeValid(DateTime birthday)
    {
        var today = DateTime.Today;
        var age = today.Year - birthday.Year;

        if (birthday > today.AddYears(-age))
        {
            age--;
        }

        return age >= 18;
    }
}
