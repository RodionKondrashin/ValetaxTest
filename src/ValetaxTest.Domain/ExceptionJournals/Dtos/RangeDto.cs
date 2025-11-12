namespace ValetaxTest.Domain.ExceptionJournals.Dtos;

public class RangeDto<T>
{
    public int SKip { get; set; }
    
    public int Count { get; set; }

    public List<T> Items { get; set; } = [];
}