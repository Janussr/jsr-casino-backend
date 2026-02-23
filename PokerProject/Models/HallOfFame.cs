using PokerProject.Models;

public class HallOfFame
{
    public int Id { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int WinningScore { get; set; }

    public DateTime WinDate { get; set; } = DateTime.UtcNow;
}
