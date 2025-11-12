using Microsoft.EntityFrameworkCore;
using Valetax.Application.Trees;
using ValetaxTest.Domain.Trees;

namespace ValetaxTest.Infrastructure.Postgres.Repositories;

public class TreesRepository : ITreesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TreesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Tree?> GetTreeAsync(string treeName, CancellationToken cancellationToken)
    {
        var tree = await _dbContext.Trees
            .Include(t => t.Nodes)
            .FirstOrDefaultAsync(t => t.Name == treeName, cancellationToken);

        return tree;
    }

    public async Task<long> AddTreeAsync(Tree tree, CancellationToken cancellationToken)
    {
        await _dbContext.Trees.AddAsync(tree, cancellationToken);
            
        await _dbContext.SaveChangesAsync(cancellationToken);
            
        return tree.Id;
    }

    public async Task<Node> AddNodeAsync(Node node, CancellationToken cancellationToken)
    {
        await _dbContext.Nodes.AddAsync(node, cancellationToken);
            
        await _dbContext.SaveChangesAsync(cancellationToken);
            
        return node;
    }

    public async Task<Node?> GetNodeAsync(long? parentId, long treeId, CancellationToken cancellationToken)
    {
        var node = await _dbContext.Nodes
            .FirstOrDefaultAsync(n => n.Id == parentId && n.TreeId == treeId, cancellationToken);

        return node;
    }

    public async Task<long> RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken)
    {
        await _dbContext.Nodes
            .Where(n => n.Id == nodeId)
            .ExecuteUpdateAsync(setter => 
                setter.SetProperty(n => n.Name, newNodeName), cancellationToken);

        return nodeId;
    }

    public async Task<long> DeleteNodeAsync(long nodeId, CancellationToken cancellationToken)
    {
        await _dbContext.Nodes
            .Where(n => n.Id == nodeId)
            .ExecuteDeleteAsync(cancellationToken);

        return nodeId;
    }
}