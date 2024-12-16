using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;
using EventsWebApplication.Domain.Abstractions.Data;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Queries.GetCurrentUserInfo;

public class GetCurrentUserInfoQueryHandler : IRequestHandler<GetCurrentUserInfoQuery, UserInfoDTO>
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetCurrentUserInfoQueryHandler(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _httpClient = httpClientFactory.CreateClient("IdentityClient");
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserInfoDTO> Handle(GetCurrentUserInfoQuery query, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Users/{query.UserId}");
        Console.WriteLine("REQUEST:");
        Console.WriteLine(_httpClient.BaseAddress);
        Console.WriteLine(request.RequestUri);
        Console.WriteLine(query.Token);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", query.Token);

        var responseMessage = await _httpClient.SendAsync(request, cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get user data. Status Code: {responseMessage.StatusCode}");
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        var userInfo = JsonSerializer.Deserialize<UserInfoDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (userInfo == null)
        {
            throw new Exception("Failed to deserialize user data.");
        }

        return userInfo;
    }
}
