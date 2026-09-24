using DirectoryService.Contracts.Positions;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PositionsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreatePositionDto input,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }

    [HttpGet("{positionId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid positionId,
        CancellationToken cancellationToken = default)
    {
        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        CancellationToken cancellationToken = default)
    {
        return Ok(Array.Empty<object>());
    }

    [HttpPut("{positionId:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid positionId,
        [FromBody] UpdatePositionDto input,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }
    
    [HttpDelete("{positionId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid positionId,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}