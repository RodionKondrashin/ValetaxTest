using Microsoft.EntityFrameworkCore;
using Valetax.Application.ExceptionJournals;
using ValetaxTest.Domain.ExceptionJournals;

namespace ValetaxTest.Infrastructure.Postgres.Repositories;

public class ExceptionJournalsRepository : IExceptionJournalsRepository
{
    private readonly JournalDbContext _dbContext;

    public ExceptionJournalsRepository(JournalDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(ExceptionJournal journal)
    {
        await _dbContext.ExceptionJournals.AddAsync(journal);
    }

    public async Task<ExceptionJournal?> GetByEventId(long eventId)
    {
        return await _dbContext.ExceptionJournals
            .FirstOrDefaultAsync(j => j.EventId == eventId);
    }

    public async Task<(int totalCount, List<ExceptionJournal> items)> GetRangeAsync(int skip, int take, DateTime? from, DateTime? to)
    {
        var query = _dbContext.ExceptionJournals.AsQueryable();

        if (from.HasValue)
        {
            query = query.Where(j => j.CreatedAt >= from.Value);
        }
        
        if (to.HasValue)
        {
            query = query.Where(j => j.CreatedAt <= to.Value);
        }
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        
        return (totalCount, items);
    }
}