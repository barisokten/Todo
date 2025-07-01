
namespace TodoApi.Models
{
    public class Board
    {
        public Board()
        {
            Lists = new List<ListModel>(); // Property'yi başlat
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<ListModel>? Lists { get; set; }
        public string UserId { get; set; } // JWT ile gelen kullanıcı

    }
}