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
    public async Task<IActionResult> GetTree([FromBody] GetTreeByNameRequest request,
        CancellationToken cancellationToken)
    {
        var tree = await _treesService.GetTreeAsync(request, cancellationToken);
        
        return Ok(tree);
    }

    [HttpPost("node.create")]
    public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequest request,
        CancellationToken cancellationToken)
    {
        await _treesService.CreateNodeAsync(request, cancellationToken);

        return Ok();
    }

    [HttpPost("node.rename")]
    public async Task<IActionResult> RenameNode([FromBody] RenameNodeRequest request,
        CancellationToken cancellationToken)
    {
        await _treesService.RenameNodeAsync(request, cancellationToken);
        
        return Ok();
    }

    [HttpPost("node.delete")]
    public async Task<IActionResult> DeleteNode([FromBody] DeleteNodeRequest request,
        CancellationToken cancellationToken)
    {
        await _treesService.DeleteNodeAsync(request, cancellationToken);
        
        return Ok();
    }
}