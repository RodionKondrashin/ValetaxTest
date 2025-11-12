using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Valetax.Application.Trees;

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
        [BindRequired] string treeName,
        CancellationToken cancellationToken)
    {
        var tree = await _treesService.GetTreeAsync(treeName, cancellationToken);
        
        return Ok(tree);
    }

    [HttpPost("node.create")]
    public async Task<IActionResult> CreateNode(
        [BindRequired] string treeName,
        [FromQuery] long? parentNodeId,
        [BindRequired] string nodeName,
        CancellationToken cancellationToken)
    {
        await _treesService.CreateNodeAsync(treeName, parentNodeId, nodeName, cancellationToken);

        return Ok();
    }

    [HttpPost("node.rename")]
    public async Task<IActionResult> RenameNode(
        [BindRequired] long nodeId,
        [BindRequired] string newNodeName,
        CancellationToken cancellationToken)
    {
        await _treesService.RenameNodeAsync(nodeId, newNodeName, cancellationToken);
        
        return Ok();
    }

    [HttpPost("node.delete")]
    public async Task<IActionResult> DeleteNode(
        [BindRequired] long nodeId,
        CancellationToken cancellationToken)
    {
        await _treesService.DeleteNodeAsync(nodeId, cancellationToken);
        
        return Ok();
    }
}