using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Models;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- EF Core + SQLite ---
var connString = builder.Configuration.GetConnectionString("Default")
                 ?? "Data Source=todoapp.db";
builder.Services.AddDbContext<TodoDbContext>(opts =>
    opts.UseSqlite(connString));

// --- DI for repository ---
builder.Services.AddScoped<ITodoRepository, EfTodoRepository>();

// --- Controllers + FluentValidation ---
builder.Services.AddControllers()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TodoCreateDtoValidator>();

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-create/migrate DB at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
}

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("LocalDev");
// Middleware
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

// --- Validators ---
public class TodoCreateDtoValidator : AbstractValidator<TodoCreateDto>
{
    public TodoCreateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}

public class TodoUpdateDtoValidator : AbstractValidator<TodoUpdateDto>
{
    public TodoUpdateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}
