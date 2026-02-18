namespace PokerProject.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int GameNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
