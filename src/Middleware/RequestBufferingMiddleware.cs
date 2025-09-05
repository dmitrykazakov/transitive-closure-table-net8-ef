using System.Text;

namespace TransitiveClosureTable.Middleware;

/// <summary>
///     Middleware to capture the raw request body and store it in HttpContext.Items for later use.
///     Useful for logging or debugging.
/// </summary>
public class RequestBufferingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Enable buffering so the request body can be read multiple times
        context.Request.EnableBuffering();

        // Capture the body as a string and store in context.Items
        context.Items["RequestBody"] = await ReadRequestBodyAsync(context);

        // Continue to the next middleware
        await next(context);
    }

    private static async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        try
        {
            context.Request.Body.Position = 0;

            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                false,
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