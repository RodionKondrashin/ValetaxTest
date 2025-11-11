namespace ValetaxTest.Domain.Nodes;

public class Node
{
    public long Id { get; set; }

    public required string Name { get; set; }
    
    public long TreeId { get; set; }
    
    public required Tree Tree { get; set; }
    
    public long? ParentId { get; set; }
    
    public Node? Parent { get; set; }

    public List<Node> Children { get; set; } = [];
}