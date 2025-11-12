using CSharpFunctionalExtensions;
using Shared;
using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public interface ITreesRepository
{
    Task<Tree?> GetTreeAsync(string treeName, CancellationToken cancellationToken);

    Task<long> AddTreeAsync(Tree tree, CancellationToken cancellationToken);
    
    Task<Node> AddNodeAsync(Node node, CancellationToken cancellationToken);

    Task<Node?> GetNodeAsync(long? parentId, long treeId, CancellationToken cancellationToken);
    
    Task<long> RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken);
    
    Task<long> DeleteNodeAsync(long nodeId, CancellationToken cancellationToken);
}