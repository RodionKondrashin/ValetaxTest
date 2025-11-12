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

    /// <summary>
    /// Returns your entire tree. If your tree doesn't exist it will be created automatically.
    /// </summary>
    /// <param name="treeName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("get")]
    public async Task<IActionResult> GetTree(
        [BindRequired] string treeName,
        CancellationToken cancellationToken)
    {
        var tree = await _treesService.GetTreeAsync(treeName, cancellationToken);
        
        return Ok(tree);
    }

    /// <summary>
    /// Create a new node in your tree.
    /// You must to specify a parent node ID that belongs to your tree or dont pass parent ID to create tree first level node.
    /// A new node name must be unique across all siblings.
    /// </summary>
    /// <param name="treeName"></param>
    /// <param name="parentNodeId"></param>
    /// <param name="nodeName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Rename an existing node in your tree. A new name of the node must be unique across all siblings.
    /// </summary>
    /// <param name="nodeId"></param>
    /// <param name="newNodeName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("node.rename")]
    public async Task<IActionResult> RenameNode(
        [BindRequired] long nodeId,
        [BindRequired] string newNodeName,
        CancellationToken cancellationToken)
    {
        await _treesService.RenameNodeAsync(nodeId, newNodeName, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Delete an existing node and all its descendants
    /// </summary>
    /// <param name="nodeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("node.delete")]
    public async Task<IActionResult> DeleteNode(
        [BindRequired] long nodeId,
        CancellationToken cancellationToken)
    {
        await _treesService.DeleteNodeAsync(nodeId, cancellationToken);
        
        return Ok();
    }
}