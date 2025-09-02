using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

public class ExceptionJournalService(IUnitOfWorkFactory unitOfWorkFactory) : IExceptionJournalService
{
    public async Task<ExceptionJournal> LogExceptionAsync(Exception ex, HttpContext context)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        var entry = new ExceptionJournal
        {
            Timestamp = DateTime.UtcNow,
            QueryParams = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "",
            BodyParams = await ReadRequestBodyAsync(context),
            StackTrace = ex.StackTrace ?? "",
            ExceptionType = ex.GetType().Name
        };

        await unitOfWork.ExceptionJournals.AddAsync(entry);

        await unitOfWork.CommitAsync();

        //await repositoryFactory.CreateExceptionJournalRepository().AddAsync(entry);
        return entry;
    }

    private static async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        return body;
    }
}