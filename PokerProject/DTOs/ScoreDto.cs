namespace PokerProject.DTOs
{
    public class ScoreDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int GameId { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
