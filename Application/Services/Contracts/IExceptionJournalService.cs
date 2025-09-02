using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

public interface IExceptionJournalService
{
    Task<ExceptionJournal> LogExceptionAsync(Exception exception, HttpContext context);
}