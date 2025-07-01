
using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public int Order { get; set; } // kart sıralaması için
    public string UserId { get; set; } = null!;

}


