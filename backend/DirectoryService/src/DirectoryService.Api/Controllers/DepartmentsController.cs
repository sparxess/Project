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

    [HttpPut("{departmentId:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid departmentId,
        [FromBody] UpdateDepartmentDto input,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }

    [HttpDelete("{departmentId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}