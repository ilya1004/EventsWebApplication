﻿namespace EventsAppIdentityServer.Application.DTOs;

public record UserInfoDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthday { get; set; }
}

var userData = new
{
    user.Id,
    user.UserName,
    user.Email,
    user.Name,
    user.Surname,
    user.Birthday
};