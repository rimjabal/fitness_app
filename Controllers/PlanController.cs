using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FitTrack.Models;
using FitTrack.Services;

namespace FitTrack.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWeeklyPlannerService _plannerService;

        public PlanController(UserManager<ApplicationUser> userManager, IWeeklyPlannerService plannerService)
        {
            _userManager = userManager;
            _plannerService = plannerService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var dietPlan = _plannerService.GenerateWeeklyDietPlan(user);
            var workoutPlan = _plannerService.GenerateWeeklyWorkoutPlan(user);

            ViewBag.DietPlan = dietPlan;
            ViewBag.WorkoutPlan = workoutPlan;
            ViewBag.User = user;

            return View();
        }
    }
}
