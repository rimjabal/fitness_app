namespace FitTrack.Models
{
    public class Objective
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Poids", "Calories", "Protéines", etc.
        public decimal GoalValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public bool IsAchieved { get; set; }
        public DateTime? AchievedDate { get; set; }
    }
}
