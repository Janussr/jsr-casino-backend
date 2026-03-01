namespace PokerProject.Models
{
    public class GameParticipant
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int RebuyCount { get; set; }
        public int ActiveBounties { get; set; }
        public Game Game { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
