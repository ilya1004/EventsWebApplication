
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateEventCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {


        // сделать сохранение фоток


        //await _unitOfWork.EventsRepository.UpdateAsync();
    }
}
