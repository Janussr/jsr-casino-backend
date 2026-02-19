namespace PokerProject.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        public int GameNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public bool IsFinished { get; set; }
        public int? WinnerUserId { get; set; }
        public int? WinningScore { get; set; }

        public List<ScoreDto> Scores { get; set; } = new();
        public WinnerDto? Winner { get; set; }
    }
}
