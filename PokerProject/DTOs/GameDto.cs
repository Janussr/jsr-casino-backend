namespace PokerProject.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        public int GameNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public bool IsFinished { get; set; }

        public int? RebuyValue { get; set; }
        public int? BountyValue { get; set; }
        public List<ParticipantDto> Participants { get; set; } = new();
        // Liste af alle spillere i spillet
        public List<ScoreDto> Scores { get; set; } = new();

        // Vinderen (kan være null, hvis spillet ikke er færdigt)
        public WinnerDto? Winner { get; set; }
    }
}
