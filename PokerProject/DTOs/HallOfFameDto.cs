namespace PokerProject.DTOs
{
    public class HallOfFameDto
    {
        public string UserName { get; set; } = null!;
        public int WinningScore { get; set; }
        public DateTime WinDate { get; set; }
    }
}
