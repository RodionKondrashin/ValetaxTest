using CSharpFunctionalExtensions;
using Shared;
using ValetaxTest.Contracts.Trees;
using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public interface ITreesService
{
    Task<Result<TreeDto, Error>> GetTreeAsync(GetTreeByNameRequest request, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> CreateNodeAsync(CreateNodeRequest request, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> DeleteNodeAsync(DeleteNodeRequest request, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> RenameNodeAsync(RenameNodeRequest request, CancellationToken cancellationToken);
}