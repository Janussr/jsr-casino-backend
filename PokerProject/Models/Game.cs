using PokerProject.Models;

public class Game
{
    public int Id { get; set; }
    public int GameNumber { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }

    public bool IsFinished { get; set; }

    public ICollection<Score> Scores { get; set; } = new List<Score>();
    public ICollection<GameParticipant> Participants { get; set; } = new List<GameParticipant>();
    public HallOfFame? Winner { get; set; }
} 
