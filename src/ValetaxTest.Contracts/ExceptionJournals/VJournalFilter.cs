namespace ValetaxTest.Contracts.ExceptionJournals;

public record VJournalFilter(DateTime? FromDate = null, DateTime? ToDate = null);