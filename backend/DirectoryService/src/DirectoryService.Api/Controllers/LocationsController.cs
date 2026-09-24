using DirectoryService.Application.Locations;
using DirectoryService.Contracts.Locations;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController(ILocationsService locationsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateLocationDto input,
        CancellationToken cancellationToken = default)
    {
        var locationId = await locationsService.CreateAsync(input, cancellationToken);
        return Ok(locationId);
    }

    [HttpGet("{locationId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid locationId,
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

    [HttpPut("{locationId:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid locationId,
        [FromBody] UpdateLocationDto input,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }
    
    [HttpDelete("{locationId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid locationId,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}