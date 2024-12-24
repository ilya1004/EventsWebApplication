namespace EventsWebApplication.Domain.Abstractions.UserInfoProvider;

public record UserInfoResponse(
    string Id, 
    string UserName, 
    string Email, 
    string Name, 
    string Surname, 
    DateTime Birthday);
