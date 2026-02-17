namespace PokerProject.Models
{
    public class HallOfFame
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public int WinningScore { get; set; }
        public DateTime WinDate { get; set; }

        // Navigation properties
        public Session Session { get; set; } = null!;
        public User User { get; set; } = null!;
    }

}
