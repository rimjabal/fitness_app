using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FitTrack.Data;
using FitTrack.Models;
using FitTrack.Services;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INutritionCalculator _nutritionCalculator;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, INutritionCalculator nutritionCalculator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _nutritionCalculator = nutritionCalculator;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var today = DateTime.Today;
            
            // Get today's calories
            var todayCalories = await _context.MealLogs
                .Where(m => m.UserId == userId && m.Date.Date == today)
                .SumAsync(m => m.Calories);

            // Get current weight and goal
            var latestMeasure = await _context.Measures
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Date)
                .FirstOrDefaultAsync();

            var activeObjective = await _context.Objectives
                .Where(o => o.UserId == userId && !o.IsAchieved && o.Type == "Poids")
                .OrderByDescending(o => o.StartDate)
                .FirstOrDefaultAsync();

            // Calculate progress
            var progressPercentage = 0m;
            if (latestMeasure != null && activeObjective != null)
            {
                var startWeight = user?.CurrentWeight ?? activeObjective.GoalValue + 5;
                var currentWeight = latestMeasure.Weight;
                var goalWeight = activeObjective.GoalValue;
                
                if (startWeight != goalWeight)
                {
                    progressPercentage = Math.Round(((startWeight - currentWeight) / (startWeight - goalWeight)) * 100, 1);
                    progressPercentage = Math.Max(0, Math.Min(100, progressPercentage));
                }
            }

            // Get recent meals
            var recentMeals = await _context.MealLogs
                .Include(m => m.Food)
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Date)
                .Take(5)
                .ToListAsync();

            // Get all objectives
            var objectives = await _context.Objectives
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.StartDate)
                .ToListAsync();

            // Calculate streak (consecutive days with meals logged)
            var streak = await CalculateStreak(userId);

            // Get today's hydration
            var todayHydration = await _context.HydrationLogs
                .Where(h => h.UserId == userId && h.Date.Date == today)
                .FirstOrDefaultAsync();

            // Get last 30 days of measures for weight chart
            var weightHistory = await _context.Measures
                .Where(m => m.UserId == userId && m.Date >= today.AddDays(-30))
                .OrderBy(m => m.Date)
                .Select(m => new { Date = m.Date.ToString("dd/MM"), Weight = m.Weight })
                .ToListAsync();

            // Calculate today's macro distribution
            var todayMacros = await _context.MealLogs
                .Where(m => m.UserId == userId && m.Date.Date == today)
                .GroupBy(m => 1)
                .Select(g => new
                {
                    TotalProtein = g.Sum(m => m.Protein),
                    TotalCarbs = g.Sum(m => m.Carbs),
                    TotalFat = g.Sum(m => m.Fat)
                })
                .FirstOrDefaultAsync();

            ViewBag.TodayCalories = Math.Round(todayCalories, 0);
            ViewBag.CurrentWeight = latestMeasure?.Weight ?? user?.CurrentWeight ?? 0;
            ViewBag.GoalWeight = activeObjective?.GoalValue ?? 0;
            ViewBag.ProgressPercentage = progressPercentage;
            ViewBag.RecentMeals = recentMeals;
            ViewBag.Objectives = objectives;
            ViewBag.AllFoods = await _nutritionCalculator.GetAllFoods(userId);
            ViewBag.User = user;
            ViewBag.Streak = streak;
            ViewBag.TodayHydration = todayHydration?.MillilitersConsumed ?? 0;
            ViewBag.HydrationGoal = todayHydration?.DailyGoalMl ?? 2000;
            ViewBag.WeightHistory = weightHistory;
            ViewBag.TodayMacros = todayMacros;

            return View();
        }

        private async Task<int> CalculateStreak(string userId)
        {
            var streak = 0;
            var currentDate = DateTime.Today;

            while (true)
            {
                var hasMeals = await _context.MealLogs
                    .AnyAsync(m => m.UserId == userId && m.Date.Date == currentDate);

                if (!hasMeals)
                    break;

                streak++;
                currentDate = currentDate.AddDays(-1);
            }

            return streak;
        }

        [HttpPost]
        public async Task<IActionResult> AddMeal(int foodId, decimal weight, string mealType)
        {
            var userId = _userManager.GetUserId(User);
            var macros = await _nutritionCalculator.CalculateMacros(foodId, weight);

            var mealLog = new MealLog
            {
                UserId = userId,
                FoodId = foodId,
                WeightInGrams = weight,
                Calories = macros.Calories,
                Protein = macros.Protein,
                Carbs = macros.Carbs,
                Fat = macros.Fat,
                Date = DateTime.Now,
                MealType = mealType
            };

            _context.MealLogs.Add(mealLog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomFood(string name, decimal calories, decimal protein, decimal carbs, decimal fat)
        {
            var userId = _userManager.GetUserId(User);
            
            var customFood = new FoodLibrary
            {
                Name = name,
                CaloriesPer100g = calories,
                ProteinPer100g = protein,
                CarbsPer100g = carbs,
                FatPer100g = fat,
                Category = "Personnalisé",
                UserId = userId,
                IsCustom = true
            };

            _context.FoodLibraries.Add(customFood);
            await _context.SaveChangesAsync();

            return Json(new { success = true, foodId = customFood.Id, foodName = customFood.Name });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            var userId = _userManager.GetUserId(User);
            var meal = await _context.MealLogs.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (meal != null)
            {
                _context.MealLogs.Remove(meal);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMeal(int id, int foodId, decimal weight, string mealType)
        {
            var userId = _userManager.GetUserId(User);
            var meal = await _context.MealLogs.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (meal != null)
            {
                var macros = await _nutritionCalculator.CalculateMacros(foodId, weight);
                
                meal.FoodId = foodId;
                meal.WeightInGrams = weight;
                meal.Calories = macros.Calories;
                meal.Protein = macros.Protein;
                meal.Carbs = macros.Carbs;
                meal.Fat = macros.Fat;
                meal.MealType = mealType;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddObjective(string title, string type, decimal goalValue, DateTime targetDate)
        {
            var userId = _userManager.GetUserId(User);
            var objective = new Objective
            {
                UserId = userId,
                Title = title,
                Type = type,
                GoalValue = goalValue,
                StartDate = DateTime.Now,
                TargetDate = targetDate,
                IsAchieved = false
            };

            _context.Objectives.Add(objective);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteObjective(int id)
        {
            var userId = _userManager.GetUserId(User);
            var objective = await _context.Objectives.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (objective != null)
            {
                _context.Objectives.Remove(objective);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateObjective(int id, string title, string type, decimal goalValue, DateTime targetDate, bool isAchieved)
        {
            var userId = _userManager.GetUserId(User);
            var objective = await _context.Objectives.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (objective != null)
            {
                objective.Title = title;
                objective.Type = type;
                objective.GoalValue = goalValue;
                objective.TargetDate = targetDate;
                objective.IsAchieved = isAchieved;
                
                if (isAchieved && !objective.IsAchieved)
                {
                    objective.AchievedDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleObjectiveStatus(int id)
        {
            var userId = _userManager.GetUserId(User);
            var objective = await _context.Objectives.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (objective != null)
            {
                objective.IsAchieved = !objective.IsAchieved;
                objective.AchievedDate = objective.IsAchieved ? DateTime.Now : null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddMeasure(decimal weight, decimal? bodyFat, decimal? muscleMass, string? notes)
        {
            var userId = _userManager.GetUserId(User);
            var measure = new Measure
            {
                UserId = userId,
                Date = DateTime.Now,
                Weight = weight,
                BodyFatPercentage = bodyFat,
                MuscleMass = muscleMass,
                Notes = notes
            };

            _context.Measures.Add(measure);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> CalculateMacros(int foodId, decimal weight)
        {
            try
            {
                var macros = await _nutritionCalculator.CalculateMacros(foodId, weight);
                return Json(new { success = true, data = macros });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetFoodInfo(int foodId)
        {
            var food = await _nutritionCalculator.GetFoodById(foodId);
            if (food == null)
            {
                return Json(new { success = false });
            }

            return Json(new { 
                success = true, 
                data = new {
                    caloriesPer100g = food.CaloriesPer100g,
                    proteinPer100g = food.ProteinPer100g,
                    carbsPer100g = food.CarbsPer100g,
                    fatPer100g = food.FatPer100g
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddWater(int milliliters = 250)
        {
            var userId = _userManager.GetUserId(User);
            var today = DateTime.Today;

            var todayHydration = await _context.HydrationLogs
                .FirstOrDefaultAsync(h => h.UserId == userId && h.Date.Date == today);

            if (todayHydration == null)
            {
                todayHydration = new HydrationLog
                {
                    UserId = userId,
                    Date = today,
                    MillilitersConsumed = milliliters,
                    DailyGoalMl = 2000
                };
                _context.HydrationLogs.Add(todayHydration);
            }
            else
            {
                todayHydration.MillilitersConsumed += milliliters;
            }

            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                totalMl = todayHydration.MillilitersConsumed,
                goalMl = todayHydration.DailyGoalMl,
                percentage = Math.Round((decimal)todayHydration.MillilitersConsumed / todayHydration.DailyGoalMl * 100, 0)
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetWater()
        {
            var userId = _userManager.GetUserId(User);
            var today = DateTime.Today;

            var todayHydration = await _context.HydrationLogs
                .FirstOrDefaultAsync(h => h.UserId == userId && h.Date.Date == today);

            if (todayHydration != null)
            {
                todayHydration.MillilitersConsumed = 0;
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }
    }
}
