using System.Text;

namespace TransitiveClosureTable.Middleware;

public class RequestBufferingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Enable buffering at the start of the pipeline
        context.Request.EnableBuffering();

        // Capture the body
        context.Items["RequestBody"] = await ReadRequestBodyAsync(context);

        await next(context);
    }

    private static async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        try
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            return body;
        }
        catch
        {
            return string.Empty;
        }
    }
}