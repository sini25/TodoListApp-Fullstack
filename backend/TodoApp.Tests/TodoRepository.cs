using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TodoApp.Application.Abstractions;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using Xunit;

namespace TodoApp.Tests
{
    public class TodoRepositoryTests
    {
        private ITodoRepository GetRepository()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TodoDbContext(options);
            return new EfTodoRepository(context);
        }

        [Fact]
        public async Task AddTodo_ShouldAddTodoSuccessfully()
        {
            var repo = GetRepository();

            var todo = new TodoItem
            {
                Title = "Test Todo",
                IsDone = false,
                CreatedAtUtc = DateTime.UtcNow
            };

            await repo.AddAsync(todo);
            var allTodos = await repo.GetAllAsync();

            Assert.Single(allTodos);
            Assert.Equal("Test Todo", allTodos[0].Title);
        }

        [Fact]
        public async Task UpdateTodo_ShouldChangeTitleAndStatus()
        {
            var repo = GetRepository();

            var todo = new TodoItem
            {
                Title = "Old Title",
                IsDone = false,
                CreatedAtUtc = DateTime.UtcNow
            };

            await repo.AddAsync(todo);

            todo.Title = "New Title";
            todo.IsDone = true;

            await repo.UpdateAsync(todo);

            var updated = await repo.GetByIdAsync(todo.Id);

            Assert.Equal("New Title", updated.Title);
            Assert.True(updated.IsDone);
        }

        [Fact]
        public async Task DeleteTodo_ShouldRemoveTodo()
        {
            var repo = GetRepository();

            var todo = new TodoItem
            {
                Title = "Delete Me",
                IsDone = false,
                CreatedAtUtc = DateTime.UtcNow
            };

            await repo.AddAsync(todo);
            await repo.DeleteAsync(todo.Id);

            var allTodos = await repo.GetAllAsync();
            Assert.Empty(allTodos);
        }

        [Fact]
        public async Task GetAllTodos_ShouldReturnCorrectCount()
        {
            var repo = GetRepository();

            await repo.AddAsync(new TodoItem { Title = "One", IsDone = false, CreatedAtUtc = DateTime.UtcNow });
            await repo.AddAsync(new TodoItem { Title = "Two", IsDone = false, CreatedAtUtc = DateTime.UtcNow });

            var allTodos = await repo.GetAllAsync();

            Assert.Equal(2, allTodos.Count);
        }
    }
}
