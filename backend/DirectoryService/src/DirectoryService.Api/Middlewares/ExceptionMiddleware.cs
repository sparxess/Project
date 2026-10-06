using DirectoryService.Application.Exceptions;
using DirectoryService.Shared;

namespace DirectoryService.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context,  ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        var (statusCode, body) = exception switch
        {
            DomainException ex => (MapStatus(ex.Error.Type), (object)ex.Error),
            ValidationException ex => (StatusCodes.Status400BadRequest, (object)ex.Errors),
            _ => (StatusCodes.Status500InternalServerError, (object)DomainError.Failure(null, "Что-то пошло не так."))
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(body);
    }
    private static int MapStatus(ErrorType type) => type switch
    {
        ErrorType.NotFound   => StatusCodes.Status404NotFound,
        ErrorType.Conflict   => StatusCodes.Status409Conflict,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        _                    => StatusCodes.Status500InternalServerError
    };
}

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this WebApplication app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}