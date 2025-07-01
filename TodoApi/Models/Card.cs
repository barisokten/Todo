public class Card
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public int ListModelId { get; set; }
    public ListModel List { get; set; }

    public int Order { get; set; }
    public string UserId { get; set; } // JWT ile gelen kullanıcı

}
