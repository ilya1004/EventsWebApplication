using AutoMapper;
using AutoMapper.Configuration.Annotations;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.Data;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;

public class AddParticipantToEventCommandHandler : IRequestHandler<AddParticipantToEventCommand>
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddParticipantToEventCommandHandler(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _httpClient = httpClientFactory.CreateClient("IdentityClient");
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(AddParticipantToEventCommand command, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.EventId);

        if (eventObj == null)
        {
            throw new Exception($"Event with given ID {command.EventId} not found.");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Users/{command.UserId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", command.Token);

        var responseMessage = await _httpClient.SendAsync(request, cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get user data. Status Code: {responseMessage.StatusCode}");
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        var userModel = JsonSerializer.Deserialize<UserInfoDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (userModel == null)
        {
            throw new Exception("Failed to deserialize user data.");
        }

        var isAlreadyParticipate = await _unitOfWork.ParticipantsRepository.AnyAsync(p => p.Email == userModel.Email && p.EventId == command.EventId, cancellationToken);

        if (isAlreadyParticipate)
        {
            throw new Exception("You are alredy participating in this event");
        }

        var participant = Participant.Create(
            userModel.Email,
            eventObj,
            new Person(userModel.Name, userModel.Surname, userModel.Birthday));

        await _unitOfWork.ParticipantsRepository.AddAsync(participant, cancellationToken);

    }
}
