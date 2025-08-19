namespace TodoApp.Core.Entities;

public class TodoItem
{
    public int Id { get; set; }            // EF Core primary key
    public string Title { get; set; } = ""; // required (we'll validate)
    public bool IsDone { get; set; } = false;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? DueAtUtc { get; set; } // optional
}
