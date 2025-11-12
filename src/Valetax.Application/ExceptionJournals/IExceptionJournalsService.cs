using ValetaxTest.Contracts.ExceptionJournals;
using ValetaxTest.Domain.ExceptionJournals.Dtos;

namespace Valetax.Application.ExceptionJournals;

public interface IExceptionJournalsService
{
    Task<MRange_MJournalInfo<MJournalInfo>> GetRangeAsync(int skip, int take, VJournalFilter filter);
    
    Task<MJournal> GetSingleAsync(long id);
}