namespace ValetaxTest.Domain.ExceptionJournals.Dtos;

public class MRange_MJournalInfo<T>
{
    public int Skip { get; set; }
    
    public int Count { get; set; }

    public List<T> Items { get; set; } = [];
}