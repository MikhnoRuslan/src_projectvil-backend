using Microsoft.AspNetCore.Http;
using Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;
using System.Net;
using System.Text.Json;

namespace Projectvil.Shared.Infrastructures.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        string result;
        switch (exception)
        {
            case ClientException e:
                response.StatusCode = e.Code ?? (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(e.Message);
                break;
            case ServerException e:
                response.StatusCode = e.Code ?? (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(e.Message);
                break;
            case EntityNotFoundException e:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(e.Message);
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(exception?.Message);
                break;
        }

        await response.WriteAsync(result);
    }
}