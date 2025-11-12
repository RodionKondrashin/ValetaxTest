namespace ValetaxTest.Contracts.ExceptionJournals;

public record GetJournalsFilter(DateTime? FromDate = null, DateTime? ToDate = null);