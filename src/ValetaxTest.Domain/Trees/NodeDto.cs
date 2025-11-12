namespace ValetaxTest.Domain.Trees;

public record NodeDto(long Id, string Name, long TreeId, long? ParentId, NodeDto[] Children);