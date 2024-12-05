using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDate;
using FluentValidation;

namespace EventsWebApplication.Application.Validators.Events;

internal class GetEventsByDateQueryValidator : AbstractValidator<GetEventsByDateQuery>
{
    public GetEventsByDateQueryValidator()
    {
        RuleFor(x => x.Date)
            .Must(d => d > new DateTime(2000, 1, 1))
            .WithMessage("Date must be after 01.01.2000");

        RuleFor(x => x.PageNo)
            .InclusiveBetween(1, 999)
            .WithMessage("PageNo value must be from 1 to 999");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize value must be from 1 to 100");
    }
}
