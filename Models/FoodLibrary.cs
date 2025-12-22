namespace FitTrack.Models
{
    public class FoodLibrary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CaloriesPer100g { get; set; }
        public decimal ProteinPer100g { get; set; }
        public decimal CarbsPer100g { get; set; }
        public decimal FatPer100g { get; set; }
        public string Category { get; set; } = string.Empty; // e.g., "Protéine", "Céréales", "Légumes"
        
        // For custom user-created foods
        public string? UserId { get; set; } // Null = system food, Not null = user's custom food
        public bool IsCustom { get; set; } = false;
    }
}
