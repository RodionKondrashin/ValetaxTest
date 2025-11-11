using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared;
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
    
    public async Task<Result<Tree, Error>> GetTreeAsync(string treeName, CancellationToken cancellationToken)
    {
        var tree = await _dbContext.Trees.FirstOrDefaultAsync(t => t.Name == treeName, cancellationToken);
        if (tree is null)
        {
            return Error.NotFound("Tree not found", treeName);
        }

        return tree;
    }

    public async Task<Result<Node, Error>> AddNodeAsync(Node node, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Nodes.AddAsync(node, cancellationToken);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return node;
        }
        catch (Exception e)
        {
            return Error.Conflict("node.insert", "Failed to add node");
        }
    }

    public async Task<Result<long, Error>> RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken)
    {
        await _dbContext.Nodes
            .Where(n => n.Id == nodeId)
            .ExecuteUpdateAsync(setter => 
                setter.SetProperty(n => n.Name, newNodeName), cancellationToken);

        return nodeId;
    }

    public async Task<UnitResult<Error>> DeleteNodeAsync(Node node, CancellationToken cancellationToken)
    {
        await _dbContext.Nodes
            .Where(n => n.Id == node.Id)
            .ExecuteDeleteAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}