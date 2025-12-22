using FitTrack.Data;
using FitTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Services
{
    public interface INutritionCalculator
    {
        Task<MacroCalculationResult> CalculateMacros(int foodId, decimal weightInGrams);
        Task<FoodLibrary?> GetFoodById(int foodId);
        Task<List<FoodLibrary>> GetAllFoods(string? userId = null);
    }

    public class MacroCalculationResult
    {
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
        public string FoodName { get; set; } = string.Empty;
    }

    public class NutritionCalculator : INutritionCalculator
    {
        private readonly ApplicationDbContext _context;

        public NutritionCalculator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MacroCalculationResult> CalculateMacros(int foodId, decimal weightInGrams)
        {
            var food = await _context.FoodLibraries.FindAsync(foodId);
            
            if (food == null)
            {
                throw new ArgumentException("Aliment non trouvé");
            }

            // Formula: (BaseValue / 100) * UserInputGrams
            var result = new MacroCalculationResult
            {
                FoodName = food.Name,
                Calories = Math.Round((food.CaloriesPer100g / 100) * weightInGrams, 1),
                Protein = Math.Round((food.ProteinPer100g / 100) * weightInGrams, 1),
                Carbs = Math.Round((food.CarbsPer100g / 100) * weightInGrams, 1),
                Fat = Math.Round((food.FatPer100g / 100) * weightInGrams, 1)
            };

            return result;
        }

        public async Task<FoodLibrary?> GetFoodById(int foodId)
        {
            return await _context.FoodLibraries.FindAsync(foodId);
        }

        public async Task<List<FoodLibrary>> GetAllFoods(string? userId = null)
        {
            // Return system foods (UserId is null) + user's custom foods (UserId matches)
            var query = _context.FoodLibraries.AsQueryable();
            
            if (userId != null)
            {
                query = query.Where(f => f.UserId == null || f.UserId == userId);
            }
            else
            {
                query = query.Where(f => f.UserId == null);
            }
            
            return await query.OrderBy(f => f.Category).ThenBy(f => f.Name).ToListAsync();
        }
    }
}
