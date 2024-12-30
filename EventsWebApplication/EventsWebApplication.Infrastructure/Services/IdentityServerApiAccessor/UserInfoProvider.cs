using EventsWebApplication.Domain.Abstractions.UserInfoProvider;
using System.Net.Http.Headers;
using System.Text.Json;
namespace EventsWebApplication.Infrastructure.Services.IdentityServerApiAccessor;

public class UserInfoProvider : IUserInfoProvider
{
    private readonly HttpClient _httpClient;
    public UserInfoProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("IdentityClient");
    }

    public async Task<UserInfoResponse> GetUserInfoAsync(string userId, string token, CancellationToken cancellationToken)
    {
        //var request = new HttpRequestMessage(HttpMethod.Get, $"api/Users/{userId}");
        //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //var responseMessage = await _httpClient.SendAsync(request, cancellationToken);

        //if (!responseMessage.IsSuccessStatusCode)
        //{
        //    throw new Exception($"Failed to get user data. Status Code: {responseMessage.StatusCode}");
        //}

        //var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        //var userInfo = JsonSerializer.Deserialize<UserInfoResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //if (userInfo == null)
        //{
        //    throw new Exception("Failed to deserialize user data.");
        //}

        //return userInfo;
    }
}
