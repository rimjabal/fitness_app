namespace FitTrack.Models
{
    public class WeeklyDietPlan
    {
        public string Day { get; set; } = string.Empty;
        public List<DietMeal> Meals { get; set; } = new();
        public decimal TotalCalories { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalCarbs { get; set; }
        public decimal TotalFat { get; set; }
    }

    public class DietMeal
    {
        public string MealTime { get; set; } = string.Empty; // "Petit-déjeuner", "Déjeuner", "Dîner", "Collation"
        public string Description { get; set; } = string.Empty;
        public List<string> Foods { get; set; } = new();
        public decimal Calories { get; set; }
    }

    public class WeeklyWorkoutPlan
    {
        public string Day { get; set; } = string.Empty;
        public string WorkoutType { get; set; } = string.Empty; // "Musculation", "Cardio", "Repos", "HIIT"
        public List<Exercise> Exercises { get; set; } = new();
        public int DurationMinutes { get; set; }
    }

    public class Exercise
    {
        public string Name { get; set; } = string.Empty;
        public string Sets { get; set; } = string.Empty; // "3x12", "4x10", "30 min"
        public string Description { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
    }
}
