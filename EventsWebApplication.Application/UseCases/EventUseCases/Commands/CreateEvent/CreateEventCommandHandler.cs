
namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

internal class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateEventCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.EventsRepository.IsSameEventExists(request.Title, request.EventDateTime, request.PlaceName))
        {
            throw new Exception("Event with this Title, DateTime and Place already exists");
        }

        //var eventobj = Event.Create(request.Title, request.Description, request.EventDateTime, request.ParticipantsMaxCount, request.Image, request.PlaceName, request.CategoryName);
        


        await _unitOfWork.EventsRepository.AddAsync(eventobj, cancellationToken);
    }
}
