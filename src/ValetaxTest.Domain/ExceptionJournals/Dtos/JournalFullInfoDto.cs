namespace ValetaxTest.Domain.ExceptionJournals.Dtos;

public record JournalFullInfoDto(long Id, long EventId, DateTime CreatedAt, string Text = "");