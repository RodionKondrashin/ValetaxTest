namespace ValetaxTest.Domain.Trees;

public record MNode(long Id, string Name, long TreeId, long? ParentId, MNode[] Children);