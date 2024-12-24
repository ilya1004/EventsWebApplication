using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace EventsWebApplication.API.Middlewares;

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
            Log.Error(ex, "An exception occurred during request processing: {RequestPath}", context.Request.Path);

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
