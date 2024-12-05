using FluentValidation;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

namespace EventsWebApplication.Application.Validators.Events;

internal class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.EventDateTime)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Event date and time must be in the future.");

        RuleFor(x => x.ParticipantsMaxCount)
            .InclusiveBetween(1, 10000).WithMessage("ParticipantsMaxCount must be between 1 and 10000.");

        RuleFor(x => x.Image)
            .Must(IsValidUrl).WithMessage("Image must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.Image));

        RuleFor(x => x.PlaceName)
            .NotEmpty().WithMessage("PlaceName is required.")
            .Length(3, 200).WithMessage("PlaceName must be between 3 and 200 characters.");

        RuleFor(x => x.CategoryName)
            .Length(3, 100).WithMessage("CategoryName must be between 3 and 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.CategoryName));
    }

    private bool IsValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var _);
    }
}
