using TodoApi.Models;


public class ListModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty; // Düzeltme 1

    public int BoardId { get; set; }

    public Board Board { get; set; } = null!; // Düzeltme 2

    public ICollection<Card> Cards { get; set; } = new List<Card>(); // Düzeltme 3
    public string UserId { get; set; } // JWT ile gelen kullanıcı

}