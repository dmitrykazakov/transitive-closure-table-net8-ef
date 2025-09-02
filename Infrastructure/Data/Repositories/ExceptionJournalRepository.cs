using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class ExceptionJournalRepository(AppDbContext appDbContext) : IExceptionJournalRepository
{
    public async Task AddAsync(ExceptionJournal entry)
    {
        appDbContext.ExceptionJournals.Add(entry);
        await appDbContext.SaveChangesAsync();
    }
}