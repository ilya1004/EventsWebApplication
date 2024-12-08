using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventsAppIdentityServer.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var details = new ProblemDetails
            {
                Title = "Server error",
                Type = "Server error",
                Detail = ex.Message
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(details);

            await context.Response.WriteAsync(json);
        }
    }
}
