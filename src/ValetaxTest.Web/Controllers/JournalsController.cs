using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Valetax.Application.ExceptionJournals;
using ValetaxTest.Contracts.ExceptionJournals;
using ValetaxTest.Domain.ExceptionJournals.Dtos;
using ValetaxTest.Domain.Exceptions;

namespace ValetaxTest.Web.Controllers;

[ApiController]
[Route("api.user.journal")]
public class JournalsController : ControllerBase
{
    private readonly IExceptionJournalsService _journalsService;

    public JournalsController(IExceptionJournalsService journalsService)
    {
        _journalsService = journalsService;
    }
    
    /// <summary>
    /// Provides the pagination API. Skip means the number of items should be skipped by server. 
    /// Take means the maximum number items should be returned by server. 
    /// All fields of the filter are optional.
    /// </summary>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="take">Maximum number of items to return</param>
    /// <param name="filter">Optional filter parameters</param>
    [HttpPost("getRange")]
    public async Task<ActionResult<MRange_MJournalInfo<MJournalInfo>>> GetRange(
        [BindRequired] int skip,
        [BindRequired] int take,
        [FromBody] VJournalFilter filter)
    {
        if (skip < 0)
        {
            throw new SecureException("Range", null, "Skip parameter must be >= 0");
        }
        
        if (take <= 0 || take > 100)
        {
            throw new SecureException("Range", null, "Take parameter must be between 1 and 100");
        }
        
        var result = await _journalsService.GetRangeAsync(skip, take, filter);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Returns the information about a particular event by ID.
    /// </summary>
    /// <param name="id">Event ID</param>
    [HttpPost("getSingle")]
    public async Task<ActionResult<MJournal>> GetSingle([BindRequired] long id)
    {
        var journal = await _journalsService.GetSingleAsync(id);
        
        return Ok(journal);
    }
}