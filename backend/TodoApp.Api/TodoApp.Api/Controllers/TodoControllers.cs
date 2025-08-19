using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Models;
using TodoApp.Core.Entities;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _repo;

    public TodosController(ITodoRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        var result = items.Select(t => new TodoReadDto(t.Id, t.Title, t.IsDone, t.CreatedAtUtc, t.DueAtUtc));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoReadDto>> GetById(int id, CancellationToken ct)
    {
        var t = await _repo.GetByIdAsync(id, ct);
        if (t is null) return NotFound();
        return Ok(new TodoReadDto(t.Id, t.Title, t.IsDone, t.CreatedAtUtc, t.DueAtUtc));
    }

    [HttpPost]
    public async Task<ActionResult<TodoReadDto>> Create([FromBody] TodoCreateDto dto, CancellationToken ct)
    {
        var entity = new TodoItem { Title = dto.Title.Trim(), DueAtUtc = dto.DueAtUtc };
        var created = await _repo.AddAsync(entity, ct);
        var read = new TodoReadDto(created.Id, created.Title, created.IsDone, created.CreatedAtUtc, created.DueAtUtc);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoUpdateDto dto, CancellationToken ct)
    {
        var existing = await _repo.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        existing.Title = dto.Title.Trim();
        existing.IsDone = dto.IsDone;
        existing.DueAtUtc = dto.DueAtUtc;

        await _repo.UpdateAsync(existing, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _repo.DeleteAsync(id, ct);
        return NoContent();
    }
}
