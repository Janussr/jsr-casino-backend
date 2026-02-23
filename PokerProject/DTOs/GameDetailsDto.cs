namespace PokerProject.DTOs
{
    public class GameDetailsDto
    {
        public int Id { get; set; }
        public int GameNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public bool IsFinished { get; set; }
        public List<GameScoreboardDto> Scores { get; set; } = new();
        public WinnerDto? Winner { get; set; }
    }

}
