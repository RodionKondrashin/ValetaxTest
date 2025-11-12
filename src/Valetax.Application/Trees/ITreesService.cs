using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public interface ITreesService
{
    Task<MTree> GetTreeAsync(string treeName, CancellationToken cancellationToken);
    
    Task CreateNodeAsync(string treeName, long? parentNodeId, string nodeName, CancellationToken cancellationToken);
    
    Task DeleteNodeAsync(long nodeId, CancellationToken cancellationToken);
    
    Task RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken);
}