using PokerProject.Models;
using static Score;

namespace PokerProject.DTOs
{
    public class ScoreEntryDto
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public ScoreType Type { get; set; }
        public int? KnockedOutUserId { get; set; }
        public string? KnockedOutUserName { get; set; }
    }
}
