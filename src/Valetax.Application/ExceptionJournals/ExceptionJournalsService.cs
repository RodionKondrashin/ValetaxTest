using ValetaxTest.Contracts.ExceptionJournals;
using ValetaxTest.Domain.ExceptionJournals.Dtos;
using ValetaxTest.Domain.Exceptions;

namespace Valetax.Application.ExceptionJournals;

public class ExceptionJournalsService : IExceptionJournalsService
{
    private readonly IExceptionJournalsRepository _exceptionJournalsRepository;

    public ExceptionJournalsService(IExceptionJournalsRepository exceptionJournalsRepository)
    {
        _exceptionJournalsRepository = exceptionJournalsRepository;
    }
    
    public async Task<MRange_MJournalInfo<MJournalInfo>> GetRangeAsync(int skip, int take, VJournalFilter filter)
    {
        var (totalCount, items) = await _exceptionJournalsRepository.GetRangeAsync(skip, take, filter.FromDate, filter.ToDate);
        
        var journalInfos = items.Select(j =>
            new MJournalInfo(j.Id, j.EventId, j.CreatedAt)).ToList();

        return new MRange_MJournalInfo<MJournalInfo>
        {
            Skip = skip,
            Count = totalCount,
            Items = journalInfos
        };
    }

    public async Task<MJournal> GetSingleAsync(long id)
    {
        var journal = await _exceptionJournalsRepository.GetByEventId(id);

        if (journal == null)
        {
            throw new SecureException("ExceptionJournal", id, $"Event with ID {id} not found");
        }

        return new MJournal(journal.Id, journal.EventId, journal.CreatedAt, journal.Text);
    }
}