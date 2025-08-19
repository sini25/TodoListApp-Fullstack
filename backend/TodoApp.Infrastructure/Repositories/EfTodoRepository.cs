using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

public class EfTodoRepository : ITodoRepository
{
    private readonly TodoDbContext _db;
    public EfTodoRepository(TodoDbContext db) => _db = db;

    public async Task<TodoItem?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Todos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default)
        => await _db.Todos.AsNoTracking().OrderByDescending(t => t.Id).ToListAsync(ct);

    public async Task<TodoItem> AddAsync(TodoItem item, CancellationToken ct = default)
    {
        _db.Todos.Add(item);
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task UpdateAsync(TodoItem item, CancellationToken ct = default)
    {
        _db.Todos.Update(item);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null) return;
        _db.Todos.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }
}
