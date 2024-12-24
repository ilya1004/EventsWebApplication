using EventsWebApplication.Application.Exceptions;
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

            var statusCode = ex switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                AlreadyExistsException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ForbiddenException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var details = new ProblemDetails
            {
                Title = statusCode == StatusCodes.Status500InternalServerError ? "Internal Server Error" : "Error",
                Type = ex.GetType().Name,
                Status = statusCode,
                Detail = ex.Message,
                Instance = context.Request.Path 
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(details);

            await context.Response.WriteAsync(json);
        }
    }
}
