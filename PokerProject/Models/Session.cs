namespace PokerProject.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime SessionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Game> Games { get; set; } = new List<Game>();
        public ICollection<HallOfFame> HallOfFames { get; set; } = new List<HallOfFame>();
    }

}
