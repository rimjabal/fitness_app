using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FitTrack.Models;

namespace FitTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Calculate daily targets based on user info
                var dailyCalories = CalculateDailyCalories(model.Weight, model.Height, model.Age, model.Gender, model.ActivityLevel, model.FitnessGoal);
                var macros = CalculateMacros(dailyCalories, model.FitnessGoal);

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    CurrentWeight = model.Weight,
                    Height = model.Height,
                    Age = model.Age,
                    Gender = model.Gender,
                    ActivityLevel = model.ActivityLevel,
                    FitnessGoal = model.FitnessGoal,
                    WorkoutsPerWeek = model.WorkoutsPerWeek,
                    RegistrationDate = DateTime.Now,
                    DailyCalorieTarget = dailyCalories,
                    DailyProteinTarget = macros.protein,
                    DailyCarbsTarget = macros.carbs,
                    DailyFatTarget = macros.fat
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Dashboard");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Calculate daily calorie needs using Mifflin-St Jeor Equation
        private decimal CalculateDailyCalories(decimal weight, decimal? height, int age, string gender, string activityLevel, string fitnessGoal)
        {
            decimal bmr;
            var heightInCm = height ?? 170; // Default height if not provided

            // Mifflin-St Jeor Equation
            if (gender == "Homme")
            {
                bmr = (10 * weight) + (6.25m * heightInCm) - (5 * age) + 5;
            }
            else
            {
                bmr = (10 * weight) + (6.25m * heightInCm) - (5 * age) - 161;
            }

            // Apply activity multiplier
            decimal activityMultiplier = activityLevel switch
            {
                "Sédentaire" => 1.2m,
                "Légèrement actif" => 1.375m,
                "Modérément actif" => 1.55m,
                "Très actif" => 1.725m,
                "Extrêmement actif" => 1.9m,
                _ => 1.2m
            };

            var tdee = bmr * activityMultiplier;

            // Adjust for fitness goal
            return fitnessGoal switch
            {
                "Perte de poids" => tdee - 500, // 500 calorie deficit
                "Gain musculaire" => tdee + 300, // 300 calorie surplus
                "Performance" => tdee + 200,
                _ => tdee // Maintenance
            };
        }

        private (decimal protein, decimal carbs, decimal fat) CalculateMacros(decimal calories, string fitnessGoal)
        {
            // Macronutrient ratios based on fitness goal
            var (proteinPercent, carbsPercent, fatPercent) = fitnessGoal switch
            {
                "Perte de poids" => (0.35m, 0.35m, 0.30m), // High protein for satiety
                "Gain musculaire" => (0.30m, 0.40m, 0.30m), // Balanced with more carbs
                "Performance" => (0.25m, 0.50m, 0.25m), // High carbs for energy
                _ => (0.30m, 0.40m, 0.30m) // Maintenance - balanced
            };

            var proteinCalories = calories * proteinPercent;
            var carbsCalories = calories * carbsPercent;
            var fatCalories = calories * fatPercent;

            // Convert calories to grams (protein: 4 cal/g, carbs: 4 cal/g, fat: 9 cal/g)
            return (
                protein: Math.Round(proteinCalories / 4, 0),
                carbs: Math.Round(carbsCalories / 4, 0),
                fat: Math.Round(fatCalories / 9, 0)
            );
        }
    }

    public class RegisterViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ActivityLevel { get; set; } = string.Empty;
        public string FitnessGoal { get; set; } = string.Empty;
        public int WorkoutsPerWeek { get; set; }
    }

    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
