namespace ValetaxTest.Domain.Trees;

public class Tree
{
    public long Id { get; set; }
    
    public required string Name { get; set; }

    public List<Node> Nodes { get; set; } = [];
}