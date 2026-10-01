using DirectoryService.Application.Departments;
using DirectoryService.Contracts.Departments;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentsController(IDepartmentsService departmentsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateDepartmentDto input,
        CancellationToken cancellationToken = default)
    {
        var departmentId = await departmentsService.CreateAsync(input, cancellationToken);
        return Ok(departmentId);
    }

    [HttpGet("{departmentId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid departmentId,
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

    [HttpPatch("{departmentId:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid departmentId,
        [FromBody] UpdateDepartmentDto input,
        CancellationToken cancellationToken = default)
    {
        await departmentsService.UpdateAsync(departmentId, input, cancellationToken);
        return Ok();
    }

    [HttpDelete("{departmentId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }

    [HttpPost("{departmentId:guid}/locations/{locationId:guid}")]
    public async Task<IActionResult> AddLocationAsync(
        [FromRoute] Guid departmentId,
        [FromRoute] Guid locationId,
        CancellationToken cancellationToken = default)
    {
        await departmentsService.AddLocationAsync(departmentId, locationId, cancellationToken);
        return Ok();
    }

    [HttpDelete("{departmentId:guid}/locations/{locationId:guid}")]
    public async Task<IActionResult> RemoveLocationAsync(
        [FromRoute] Guid departmentId,
        [FromRoute] Guid locationId,
        CancellationToken cancellationToken = default)
    {
        await departmentsService.RemoveLocationAsync(departmentId, locationId, cancellationToken);
        return Ok();
    }
}