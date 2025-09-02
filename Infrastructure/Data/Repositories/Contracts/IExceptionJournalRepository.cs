using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

public interface IExceptionJournalRepository
{
    Task AddAsync(ExceptionJournal entry);
}