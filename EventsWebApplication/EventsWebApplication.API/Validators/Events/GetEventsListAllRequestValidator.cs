using EventsWebApplication.API.Contracts.Events;
using FluentValidation;

namespace EventsWebApplication.API.Validators.Events;

public class GetEventsListAllRequestValidator : AbstractValidator<GetEventsListAllRequest>
{
    public GetEventsListAllRequestValidator()
    {
        RuleFor(x => x.PageNo)
            .InclusiveBetween(1, 999)
            .WithMessage("PageNo value must be from 1 to 999");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize value must be from 1 to 100");
    }
}
