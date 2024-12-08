using FluentValidation;
using EventsWebApplication.API.Contracts.Events;

namespace EventsWebApplication.API.Validators.Events;

internal class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.EventDTO.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

        RuleFor(x => x.EventDTO.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.EventDTO.EventDateTime)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Event date and time must be in the future.");

        RuleFor(x => x.EventDTO.ParticipantsMaxCount)
            .InclusiveBetween(1, 10000).WithMessage("ParticipantsMaxCount must be between 1 and 10000.");

        RuleFor(x => x.EventDTO.ImageFile!.FileName)
            .Must(IsValidUrl).WithMessage("Image must be a valid URL.")
            .When(x => x.EventDTO.ImageFile != null);

        RuleFor(x => x.EventDTO.PlaceName)
            .NotEmpty().WithMessage("PlaceName is required.")
            .Length(3, 200).WithMessage("PlaceName must be between 3 and 200 characters.");

        RuleFor(x => x.EventDTO.CategoryName)
            .Length(3, 100).WithMessage("CategoryName must be between 3 and 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.EventDTO.CategoryName));
    }

    private bool IsValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var _);
    }
}
