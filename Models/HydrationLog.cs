namespace FitTrack.Models
{
    public class HydrationLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int MillilitersConsumed { get; set; }
        public int DailyGoalMl { get; set; } = 2000; // Default 2L per day
    }
}
