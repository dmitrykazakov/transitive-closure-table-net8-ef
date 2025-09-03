using System.Net;
using System.Text.Json;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Exceptions;

namespace TransitiveClosureTable.Middleware;

/// <summary>
/// Middleware to handle exceptions globally and log them to the ExceptionJournal.
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware pipeline and catches unhandled exceptions.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exceptionJournalService">Service for logging exceptions.</param>
    public async Task InvokeAsync(HttpContext context, IExceptionJournalService exceptionJournalService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log exception to database and get event ID
            var eventEntry = await exceptionJournalService.LogExceptionAsync(ex, context);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // Prepare response depending on exception type
            var response = ex switch
            {
                SecureException => new
                {
                    type = "Secure",
                    id = eventEntry.EventId,
                    data = new { message = ex.Message }
                },
                _ => new
                {
                    type = "Exception",
                    id = eventEntry.EventId,
                    data = new { message = $"Internal server error ID = {eventEntry.EventId}" }
                }
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}

/// <summary>
/// Extension method to add the middleware to the pipeline.
/// </summary>
public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
