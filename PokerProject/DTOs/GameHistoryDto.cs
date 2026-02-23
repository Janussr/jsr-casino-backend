namespace PokerProject.DTOs
{
    public class GameHistoryDto
    {
        public DateTime PlayedAt { get; set; }
        public string Winner { get; set; }
        public int WinnerScore { get; set; }
        public List<ScoreDto> Scores { get; set; }
    }
}
