using System.Text;

namespace TransitiveClosureTable.Middleware;

/// <summary>
/// Middleware to capture the raw request body and store it in HttpContext.Items for later use.
/// Useful for logging or debugging.
/// </summary>
public class RequestBufferingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestBufferingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Enable buffering so the request body can be read multiple times
        context.Request.EnableBuffering();

        // Capture the body as a string and store in context.Items
        context.Items["RequestBody"] = await ReadRequestBodyAsync(context);

        // Continue to the next middleware
        await _next(context);
    }

    private static async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        try
        {
            context.Request.Body.Position = 0;

            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true
            );

            var body = await reader.ReadToEndAsync();

            // Reset the stream position so downstream middleware/controllers can read it
            context.Request.Body.Position = 0;

            return body;
        }
        catch
        {
            return string.Empty;
        }
    }
}

/// <summary>
/// Extension method to add the middleware to the pipeline.
/// </summary>
public static class RequestBufferingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestBufferingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestBufferingMiddleware>();
    }
}