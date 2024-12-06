
using AutoMapper;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

internal class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.EventsRepository.IsSameEventExists(request.EventDTO.Title, 
                                                                 request.EventDTO.EventDateTime, 
                                                                 request.EventDTO.PlaceName))
        {
            throw new Exception("Event with this Title, DateTime and Place already exists");
        }

        //var eventEntity = _mapper.Map<Event>(request);

        // сделать сохранение фоток

        //await _unitOfWork.EventsRepository.AddAsync(eventEntity, cancellationToken);
    }
}
