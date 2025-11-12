namespace ValetaxTest.Domain.ExceptionJournals.Dtos;

public record MJournal(long Id, long EventId, DateTime CreatedAt, string Text = "");