namespace FitTrack.Models
{
    public class MealLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int FoodId { get; set; }
        public FoodLibrary? Food { get; set; }
        public decimal WeightInGrams { get; set; }
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
        public DateTime Date { get; set; }
        public string MealType { get; set; } = string.Empty; // "Petit-déjeuner", "Déjeuner", "Dîner", "Collation"
    }
}
