using ValetaxTest.Contracts.ExceptionJournals;
using ValetaxTest.Domain.ExceptionJournals.Dtos;

namespace Valetax.Application.ExceptionJournals;

public interface IExceptionJournalsService
{
    Task<RangeDto<JournalDto>> GetRangeAsync(int skip, int take, GetJournalsFilter filter);
    
    Task<JournalFullInfoDto> GetSingleAsync(long id);
}