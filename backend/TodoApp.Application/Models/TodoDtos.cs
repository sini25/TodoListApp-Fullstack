namespace TodoApp.Application.Models;

public record TodoCreateDto(string Title, DateTime? DueAtUtc);
public record TodoUpdateDto(string Title, bool IsDone, DateTime? DueAtUtc);
public record TodoReadDto(int Id, string Title, bool IsDone, DateTime CreatedAtUtc, DateTime? DueAtUtc);
