using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

public class ExceptionJournalService(IUnitOfWorkFactory unitOfWorkFactory) : IExceptionJournalService
{
    public async Task<ExceptionJournal> LogExceptionAsync(Exception ex, HttpContext context)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        var exceptionJournal = new ExceptionJournal
        {
            Timestamp = DateTime.UtcNow,
            QueryParams = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "",
            BodyParams = context.Items["RequestBody"] as string ?? string.Empty,
            StackTrace = ex.StackTrace ?? "",
            ExceptionType = ex.GetType().Name
        };

        await unitOfWork.ExceptionJournals.AddAsync(exceptionJournal);

        await unitOfWork.CommitAsync();

        return exceptionJournal;
    }

}