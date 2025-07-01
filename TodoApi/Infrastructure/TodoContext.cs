using Microsoft.EntityFrameworkCore;
using TodoApi.Models;



    public class TodoContext : DbContext
    {
        public TodoContext() : base() { }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<Board> Boards { get; set; }
        public DbSet<ListModel> Lists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
