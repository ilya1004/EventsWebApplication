namespace EventsWebApplication.Domain.Abstractions.UserInfoProvider;

public interface IUserInfoProvider
{
    public Task<UserInfoResponse> GetUserInfoAsync(string userId, string token, CancellationToken cancellationToken);
}
