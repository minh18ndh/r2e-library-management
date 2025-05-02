using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using LibraryManagement.Api.Models;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Api.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        var statusCode = GetExceptionResponseStatusCode(exception);
        var message = GetExceptionResponseMessage(exception) ?? "Unexpected error";

        var errorResponse = new Result
        {
            StatusCode = statusCode,
            IsSuccess = false,
            Errors = new List<Error>
            {
                new Error(message, exception.Message)
            }
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        return true;
    }

    private static int GetExceptionResponseStatusCode(Exception exception) => exception switch
    {
        BadRequestException => 400,
        NotFoundException => 404,
        ValidationException => 400,
        UnAuthorizedException => 401,
        InternalServerErrorException => 500,
        _ => 400
    };

    private static string GetExceptionResponseMessage(Exception exception) => exception switch
    {
        BadRequestException => "Bad request",
        NotFoundException => "Not found",
        ValidationException => "Invalid model",
        UnAuthorizedException => "Unauthorized",
        InternalServerErrorException => "Internal server error",
        _ => "Bad request"
    };
}