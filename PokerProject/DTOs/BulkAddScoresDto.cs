namespace PokerProject.DTOs
{
    public class BulkAddScoresDto
    {
        public int GameId { get; set; }
        public List<ScoreInputDto> Scores { get; set; } = new();
    }
}
