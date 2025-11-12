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

    public async Task<Result<TreeDto, Error>> GetTreeAsync(string treeName, CancellationToken cancellationToken)
    {
        var treeResult = await _treesRepository.GetTreeAsync(treeName, cancellationToken);
        if (treeResult.Error.Type == ErrorType.NOT_FOUND)
        {
            var newTree = new Tree
            {
                Name = treeName
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

    public async Task<UnitResult<Error>> CreateNodeAsync(string treeName, long? parentNodeId, string nodeName, CancellationToken cancellationToken)
    {
        var tree = await GetTreeAsync(treeName, cancellationToken);
        if (tree.IsFailure)
        {
            return tree.Error;
        }

        if (parentNodeId is not null)
        {
            var parent = await _treesRepository.GetNodeAsync(parentNodeId, tree.Value.Id, cancellationToken);
            if (parent.IsFailure)
            {
                return parent.Error;
            }
        }
        
        var newNode = new Node
        {
            Name = treeName,
            TreeId = tree.Value.Id,
            ParentId = parentNodeId,
        };

        var result = await _treesRepository.AddNodeAsync(newNode, cancellationToken);
        
        return result.IsFailure ? result.Error : UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteNodeAsync(long nodeId, CancellationToken cancellationToken)
    {
        var result = await _treesRepository.DeleteNodeAsync(nodeId, cancellationToken);
        
        return result.IsFailure ? result.Error : UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken)
    {
        var result = await _treesRepository.RenameNodeAsync(nodeId, newNodeName, cancellationToken);
        
        return result.IsFailure ? result.Error : UnitResult.Success<Error>();
    }
}