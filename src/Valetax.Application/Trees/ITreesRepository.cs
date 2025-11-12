using CSharpFunctionalExtensions;
using Shared;
using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public interface ITreesRepository
{
    Task<Result<Tree, Error>> GetTreeAsync(string treeName, CancellationToken cancellationToken);

    Task<Result<long, Error>> AddTreeAsync(Tree tree, CancellationToken cancellationToken);
    
    Task<Result<Node, Error>> AddNodeAsync(Node node, CancellationToken cancellationToken);

    Task<Result<Node, Error>> GetNodeAsync(long parentId, long treeId, CancellationToken cancellationToken);
    
    Task<Result<long, Error>> RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> DeleteNodeAsync(long nodeId, CancellationToken cancellationToken);
}