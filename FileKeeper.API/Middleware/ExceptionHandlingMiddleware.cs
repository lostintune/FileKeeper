using FileKeeper.Application.Common.Exceptions;
using FileKeeper.Application.Common.Exceptions.Files;
using FileKeeper.Domain.Exceptions;

namespace FileKeeper.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            var statusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                ForbiddenAccessException => StatusCodes.Status403Forbidden,
                InvalidCredentialsException => StatusCodes.Status401Unauthorized,
                DomainException => StatusCodes.Status400BadRequest,
                IdentityOperationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            
            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(ex, "Unhandled exception");
            }

            var message = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : ex.Message;
            
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}