using ValetaxTest.Domain.ExceptionJournals;

namespace Valetax.Application.ExceptionJournals;

public interface IExceptionJournalsRepository
{
    Task AddAsync(ExceptionJournal journal);

    Task<ExceptionJournal?> GetByEventId(long eventId);
    
    Task<(int totalCount, List<ExceptionJournal> items)> GetRangeAsync(
        int skip,
        int take,
        DateTime? from,
        DateTime? to);
}