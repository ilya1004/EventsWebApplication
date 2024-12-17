using EventsWebApplication.API.Contracts.Events;
using FluentValidation;

namespace EventsWebApplication.API.Validators.Events;

public class GetEventsByTitleRequestValidator : AbstractValidator<GetEventsByTitleRequest>
{
    public GetEventsByTitleRequestValidator()
    {
        RuleFor(x => x.TitleQuery)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

        RuleFor(x => x.PageNo)
            .InclusiveBetween(1, 999)
            .WithMessage("PageNo value must be from 1 to 999");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize value must be from 1 to 100");
    }
}
