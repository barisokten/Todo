using Microsoft.EntityFrameworkCore;

public class TodoContext : DbContext
{
    public TodoContext() : base(){}

    public TodoContext(DbContextOptions<TodoContext> options) : base(options){}

    public DbSet<TodoItem> TodoItems {get; set;}
}
