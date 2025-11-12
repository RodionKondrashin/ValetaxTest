using CSharpFunctionalExtensions;
using Shared;
using ValetaxTest.Contracts.Trees;
using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public interface ITreesService
{
    Task<Result<TreeDto, Error>> GetTreeAsync(string treeName, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> CreateNodeAsync(string treeName, long? parentNodeId, string nodeName, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> DeleteNodeAsync(long nodeId, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken);
}