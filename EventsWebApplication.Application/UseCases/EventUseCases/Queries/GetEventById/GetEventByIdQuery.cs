using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Primitives;
using System.Linq.Expressions;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;

public sealed record GetEventByIdQuery(int EventId, params Expression<Func<Event, object>>[]? IncludesProperties) : IRequest<Event>;
