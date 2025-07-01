using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todoitems
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTodoItems()
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            var items = await _context.TodoItems
                .Where(item => item.UserId == userId)
                .OrderBy(item => item.Order)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/todoitems/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            var item = await _context.TodoItems.FindAsync(id);
            if (item == null || item.UserId != userId)
                return NotFound();

            return Ok(item);
        }

        // POST: api/todoitems
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTodoItem([FromBody] TodoItem newItem)
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            if (newItem.DueDate.HasValue)
                newItem.DueDate = newItem.DueDate.Value.ToUniversalTime();

            var maxOrder = await _context.TodoItems
                .Where(i => i.UserId == userId)
                .MaxAsync(i => (int?)i.Order) ?? 0;

            newItem.Order = maxOrder + 1;
            newItem.UserId = userId;

            _context.TodoItems.Add(newItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = newItem.Id }, newItem);
        }

        // DELETE: api/todoitems/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null || todoItem.UserId != userId)
                return NotFound();

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/todoitems/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTodoItem(int id, [FromBody] TodoItem updatedItem)
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            var todo = await _context.TodoItems.FindAsync(id);
            if (todo == null || todo.UserId != userId)
                return NotFound();

            todo.Title = updatedItem.Title;
            todo.DueDate = updatedItem.DueDate;
            todo.IsCompleted = updatedItem.IsCompleted;
            todo.Order = updatedItem.Order;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/todoitems/reorder
        [HttpPut("reorder")]
        [Authorize]
        public async Task<IActionResult> ReorderTodos([FromBody] List<TodoItem> updatedItems)
        {
            var userId = User.FindFirst("user_id")?.Value;
            if (userId == null) return Unauthorized();

            foreach (var item in updatedItems)
            {
                var existing = await _context.TodoItems.FindAsync(item.Id);
                if (existing != null && existing.UserId == userId)
                {
                    existing.Order = item.Order;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
