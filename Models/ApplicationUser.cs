using Microsoft.AspNetCore.Identity;

namespace FitTrack.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public decimal CurrentWeight { get; set; }
        public decimal? Height { get; set; } // in cm
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty; // "Homme", "Femme"
        public string ActivityLevel { get; set; } = string.Empty; // "Sédentaire", "Légèrement actif", "Modérément actif", "Très actif", "Extrêmement actif"
        public string FitnessGoal { get; set; } = string.Empty; // "Perte de poids", "Gain musculaire", "Maintenance", "Performance"
        public int WorkoutsPerWeek { get; set; }
        public DateTime RegistrationDate { get; set; }
        
        // Calculated daily targets (can be recalculated based on user info)
        public decimal DailyCalorieTarget { get; set; }
        public decimal DailyProteinTarget { get; set; }
        public decimal DailyCarbsTarget { get; set; }
        public decimal DailyFatTarget { get; set; }
    }
}
