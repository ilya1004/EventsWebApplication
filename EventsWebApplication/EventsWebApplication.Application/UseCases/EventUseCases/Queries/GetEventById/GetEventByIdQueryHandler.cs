﻿using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.Data;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;

internal class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Event>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEventByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Event> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(
            query.EventId,
            cancellationToken,
            query.IncludesProperties);
        
        if (eventObj == null)
        {
            throw new Exception($"Event with ID {query.EventId} not found.");
        }

        return eventObj;
    }
}