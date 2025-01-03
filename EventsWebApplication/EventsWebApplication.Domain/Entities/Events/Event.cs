﻿using EventsWebApplication.Domain.Entities.Participants;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Events;

public class Event : Entity
{
    private readonly List<Participant> _participants = [];
    public Event() : base(0) { }
    public Event(
       string title,
       string? description,
       DateTime eventDateTime,
       int participantsMaxCount,
       string? image,
       Place place,
       Category? category) : base(0)
    {
        Title = title;
        Description = description;
        EventDateTime = eventDateTime;
        ParticipantsMaxCount = participantsMaxCount;
        Image = image;
        Place = place;
        Category = category;
    }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public int ParticipantsMaxCount { get; set; }
    public string? Image { get; set; }
    public Place Place { get; set; }
    public Category? Category { get; set; }
    public IReadOnlyList<Participant> Participants { get => _participants.AsReadOnly(); }

}
