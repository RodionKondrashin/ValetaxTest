using CSharpFunctionalExtensions;
using Shared;
using ValetaxTest.Contracts.Trees;
using ValetaxTest.Domain.Exceptions;
using ValetaxTest.Domain.Trees;

namespace Valetax.Application.Trees;

public class TreesService : ITreesService
{
    private readonly ITreesRepository _treesRepository;

    public TreesService(ITreesRepository treesRepository)
    {
        _treesRepository = treesRepository;
    }

    public async Task<TreeDto> GetTreeAsync(string treeName, CancellationToken cancellationToken)
    {
        var tree = await _treesRepository.GetTreeAsync(treeName, cancellationToken);
        if (tree is null)
        {
            var newTree = new Tree
            {
                Name = treeName
            };
            
            await _treesRepository.AddTreeAsync(newTree, cancellationToken);

            return new TreeDto(newTree.Id, newTree.Name, []);
        }
        
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

    public async Task CreateNodeAsync(string treeName, long? parentNodeId, string nodeName, CancellationToken cancellationToken)
    {
        var tree = await GetTreeAsync(treeName, cancellationToken);

        if (parentNodeId is not null)
        {
            var parent = await _treesRepository.GetNodeAsync(parentNodeId, tree.Id, cancellationToken);
            if (parent is null)
            {
                throw new SecureException("Node", parentNodeId.Value, 
                    $"Parent node {parentNodeId.Value} not found in tree '{treeName}'");
            }
        }
        
        var newNode = new Node
        {
            Name = treeName,
            TreeId = tree.Id,
            ParentId = parentNodeId,
        };

        var node = await _treesRepository.AddNodeAsync(newNode, cancellationToken);
    }

    public async Task DeleteNodeAsync(long nodeId, CancellationToken cancellationToken)
    {
        var result = await _treesRepository.DeleteNodeAsync(nodeId, cancellationToken);
    }

    public async Task RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken)
    {
        var result = await _treesRepository.RenameNodeAsync(nodeId, newNodeName, cancellationToken);
    }
}