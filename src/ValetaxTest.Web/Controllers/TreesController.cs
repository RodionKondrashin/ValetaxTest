using Microsoft.AspNetCore.Mvc;
using Valetax.Application.Trees;
using ValetaxTest.Contracts.Trees;

namespace ValetaxTest.Web.Controllers;

[ApiController]
[Route("api.user.tree")]
public class TreesController : ControllerBase
{
    private readonly ITreesService _treesService;

    public TreesController(ITreesService treesService)
    {
        _treesService = treesService;
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetTree(
        [FromQuery] string treeName,
        CancellationToken cancellationToken)
    {
        var tree = await _treesService.GetTreeAsync(treeName, cancellationToken);
        
        return Ok(tree);
    }

    [HttpPost("node.create")]
    public async Task<IActionResult> CreateNode(
        [FromQuery] string treeName,
        [FromQuery] long? parentNodeId,
        [FromQuery] string nodeName,
        CancellationToken cancellationToken)
    {
        await _treesService.CreateNodeAsync(treeName, parentNodeId, nodeName, cancellationToken);

        return Ok();
    }

    [HttpPost("node.rename")]
    public async Task<IActionResult> RenameNode(
        [FromQuery] long nodeId,
        [FromQuery] string newNodeName,
        CancellationToken cancellationToken)
    {
        await _treesService.RenameNodeAsync(nodeId, newNodeName, cancellationToken);
        
        return Ok();
    }

    [HttpPost("node.delete")]
    public async Task<IActionResult> DeleteNode(
        [FromQuery] long nodeId,
        CancellationToken cancellationToken)
    {
        await _treesService.DeleteNodeAsync(nodeId, cancellationToken);
        
        return Ok();
    }
}