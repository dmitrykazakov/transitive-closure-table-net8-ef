using System.Net;
using System.Text.Json;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Exceptions;

namespace TransitiveClosureTable.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IExceptionJournalService exceptionJournalService)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var eventId = await exceptionJournalService.LogExceptionAsync(ex, context);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            object response;

            if (ex is SecureException)
                response = new
                {
                    type = "Secure",
                    id = eventId,
                    data = new { message = ex.Message }
                };
            else
                response = new
                {
                    type = "Exception",
                    id = eventId,
                    data = new { message = $"Internal server error ID = {eventId}" }
                };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}