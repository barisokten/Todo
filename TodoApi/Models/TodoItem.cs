
using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    [Key]
    public int ID { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
    
    public DateTime?DueDate { get; set; }
}
