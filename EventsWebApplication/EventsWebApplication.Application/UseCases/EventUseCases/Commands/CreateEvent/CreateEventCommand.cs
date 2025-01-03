﻿using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

public sealed record CreateEventCommand(
    EventDTO EventDTO,
    Stream? FileStream,
    string? ContentType) : IRequest;
