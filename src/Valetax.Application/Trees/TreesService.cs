using CSharpFunctionalExtensions;
using Shared;
using ValetaxTest.Contracts.Trees;
using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public class TreesService : ITreesService
{
    private readonly ITreesRepository _treesRepository;

    public TreesService(ITreesRepository treesRepository)
    {
        _treesRepository = treesRepository;
    }

    public async Task<Result<TreeDto, Error>> GetTreeAsync(GetTreeByNameRequest request, CancellationToken cancellationToken)
    {
        var treeResult = await _treesRepository.GetTreeAsync(request.TreeName, cancellationToken);
        if (treeResult.Error.Type == ErrorType.NOT_FOUND)
        {
            var newTree = new Tree
            {
                Name = request.TreeName
            };
            
            await _treesRepository.AddTreeAsync(newTree, cancellationToken);

            return new TreeDto(newTree.Id, newTree.Name, []);
        }
        if (treeResult.IsFailure)
        {
            return treeResult.Error;
        }
        
        var tree = treeResult.Value;
        if (tree.Nodes.Count == 0)
        {
            return new TreeDto(tree.Id, tree.Name, []);
        }

        var lookup = tree.Nodes.ToLookup(n => n.ParentId);

        var rootNodes = lookup[null];

        var rootNodeDtos = rootNodes.Select(n => MapNodeToDto(n, lookup)).ToArray();
        
        return new TreeDto(tree.Id, tree.Name, rootNodeDtos);
    }

    private NodeDto MapNodeToDto(Node node, ILookup<long?, Node> lookup)
    {
        var children = lookup[node.Id];
        
        var childrenDtos = children.Select(child => MapNodeToDto(child, lookup)).ToArray();
    
        return new NodeDto(
            node.Id,
            node.Name,
            node.TreeId,
            node.ParentId,
            childrenDtos
        );
    }

    public async Task<UnitResult<Error>> CreateNodeAsync(CreateNodeRequest request, CancellationToken cancellationToken)
    {
        var tree = await GetTreeAsync(new GetTreeByNameRequest(request.TreeName), cancellationToken);
        if (tree.IsFailure)
        {
            return tree.Error;
        }

        if (request.ParentNodeId.HasValue)
        {
            var parent = await _treesRepository.GetNodeAsync(request.ParentNodeId.Value, tree.Value.Id, cancellationToken);
            if (parent.IsFailure)
            {
                return parent.Error;
            }
        }
        
        var newNode = new Node
        {
            Name = request.TreeName,
            TreeId = tree.Value.Id,
            ParentId = request.ParentNodeId
        };

        var result = await _treesRepository.AddNodeAsync(newNode, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error;
        }

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteNodeAsync(DeleteNodeRequest request, CancellationToken cancellationToken)
    {
        var result = await _treesRepository.DeleteNodeAsync(request.NodeId, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error;
        }
        
        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> RenameNodeAsync(RenameNodeRequest request, CancellationToken cancellationToken)
    {
        var result = await _treesRepository.RenameNodeAsync(request.NodeId, request.NewNodeName, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error;
        }
        
        return UnitResult.Success<Error>();
    }
}