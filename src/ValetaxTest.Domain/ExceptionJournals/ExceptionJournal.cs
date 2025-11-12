namespace ValetaxTest.Domain.ExceptionJournals;

public class ExceptionJournal
{
    public long Id { get; set; }
    
    public long EventId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string Text { get; set; } = string.Empty;
    
    public string? QueryParameters { get; set; }
    
    public string? BodyParameters { get; set; }
    
    public string StackTrace { get; set; } = string.Empty;
}