namespace PokerProject.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int GameNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Session Session { get; set; } = null!;
        public ICollection<Score> Scores { get; set; } = new List<Score>();
    }

}
