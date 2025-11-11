using CSharpFunctionalExtensions;
using Shared;
using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public interface ITreesRepository
{
    Task<Result<Tree, Error>> GetTreeAsync(string treeName);
    
    Task<Result<Node, Error>> AddNodeAsync(string treeName, long? parentNodeId, string nodeName);
    
    Task<Result<Node, Error>> RenameNodeAsync(long nodeId, string newNodeName);
    
    Task<UnitResult<Error>> DeleteNodeAsync(long nodeId);
}