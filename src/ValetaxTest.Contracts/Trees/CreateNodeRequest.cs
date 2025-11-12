namespace ValetaxTest.Contracts.Trees;

public record CreateNodeRequest(string TreeName, long? ParentNodeId, string NodeName);