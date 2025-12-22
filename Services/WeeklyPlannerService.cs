using FitTrack.Models;

namespace FitTrack.Services
{
    public interface IWeeklyPlannerService
    {
        List<WeeklyDietPlan> GenerateWeeklyDietPlan(ApplicationUser user);
        List<WeeklyWorkoutPlan> GenerateWeeklyWorkoutPlan(ApplicationUser user);
    }

    public class WeeklyPlannerService : IWeeklyPlannerService
    {
        public List<WeeklyDietPlan> GenerateWeeklyDietPlan(ApplicationUser user)
        {
            var dailyCalories = user.DailyCalorieTarget;
            var dailyProtein = user.DailyProteinTarget;
            var dailyCarbs = user.DailyCarbsTarget;
            var dailyFat = user.DailyFatTarget;

            var days = new[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
            var weeklyPlan = new List<WeeklyDietPlan>();

            foreach (var day in days)
            {
                var plan = new WeeklyDietPlan
                {
                    Day = day,
                    TotalCalories = dailyCalories,
                    TotalProtein = dailyProtein,
                    TotalCarbs = dailyCarbs,
                    TotalFat = dailyFat,
                    Meals = GenerateDailyMeals(user, dailyCalories)
                };
                weeklyPlan.Add(plan);
            }

            return weeklyPlan;
        }

        private List<DietMeal> GenerateDailyMeals(ApplicationUser user, decimal dailyCalories)
        {
            var meals = new List<DietMeal>();

            // Petit-déjeuner (25% des calories)
            var breakfastCals = dailyCalories * 0.25m;
            meals.Add(new DietMeal
            {
                MealTime = "Petit-déjeuner",
                Description = "Repas énergétique pour bien commencer la journée",
                Foods = GetBreakfastOptions(user.FitnessGoal),
                Calories = breakfastCals
            });

            // Collation Matin (10% des calories)
            var snack1Cals = dailyCalories * 0.10m;
            meals.Add(new DietMeal
            {
                MealTime = "Collation Matin",
                Description = "En-cas léger entre les repas",
                Foods = GetSnackOptions(),
                Calories = snack1Cals
            });

            // Déjeuner (35% des calories)
            var lunchCals = dailyCalories * 0.35m;
            meals.Add(new DietMeal
            {
                MealTime = "Déjeuner",
                Description = "Repas principal de la journée",
                Foods = GetLunchOptions(user.FitnessGoal),
                Calories = lunchCals
            });

            // Collation Après-midi (10% des calories)
            var snack2Cals = dailyCalories * 0.10m;
            meals.Add(new DietMeal
            {
                MealTime = "Collation Après-midi",
                Description = "Boost d'énergie avant l'entraînement",
                Foods = GetPreWorkoutSnack(),
                Calories = snack2Cals
            });

            // Dîner (20% des calories)
            var dinnerCals = dailyCalories * 0.20m;
            meals.Add(new DietMeal
            {
                MealTime = "Dîner",
                Description = "Repas léger pour la récupération",
                Foods = GetDinnerOptions(user.FitnessGoal),
                Calories = dinnerCals
            });

            return meals;
        }

        private List<string> GetBreakfastOptions(string fitnessGoal)
        {
            return fitnessGoal switch
            {
                "Perte de poids" => new List<string>
                {
                    "🥚 3 œufs brouillés",
                    "🥑 1/2 avocat",
                    "🍞 1 tranche de pain complet",
                    "☕ Café noir / Thé vert"
                },
                "Gain musculaire" => new List<string>
                {
                    "🥚 4 œufs + 2 blancs d'œufs",
                    "🥐 100g couscous",
                    "🍌 1 banane",
                    "🥛 Verre de lait"
                },
                _ => new List<string>
                {
                    "🥣 Yaourt grec + miel",
                    "🥜 30g amandes",
                    "🍓 Fruits rouges",
                    "🍞 Pain complet"
                }
            };
        }

        private List<string> GetSnackOptions()
        {
            return new List<string>
            {
                "🍎 1 pomme",
                "🥜 20g amandes",
                "☕ Thé vert marocain"
            };
        }

        private List<string> GetLunchOptions(string fitnessGoal)
        {
            return fitnessGoal switch
            {
                "Perte de poids" => new List<string>
                {
                    "🍗 150g poulet grillé",
                    "🥗 Salade verte à volonté",
                    "🍠 100g patate douce",
                    "🫒 Huile d'olive (1 cuillère)"
                },
                "Gain musculaire" => new List<string>
                {
                    "🥩 200g bœuf haché maigre",
                    "🍚 150g riz brun",
                    "🥦 Brocoli à volonté",
                    "🥗 Salade composée"
                },
                _ => new List<string>
                {
                    "🐟 150g saumon",
                    "🌾 100g quinoa",
                    "🥬 Légumes sautés",
                    "🥗 Salade verte"
                }
            };
        }

        private List<string> GetPreWorkoutSnack()
        {
            return new List<string>
            {
                "🍌 1 banane",
                "🥛 Whey Protéine (30g)",
                "💧 Eau (500ml)"
            };
        }

        private List<string> GetDinnerOptions(string fitnessGoal)
        {
            return fitnessGoal switch
            {
                "Perte de poids" => new List<string>
                {
                    "🐟 150g poisson blanc",
                    "🥬 Légumes vapeur",
                    "🥗 Salade verte",
                    "🍵 Tisane menthe"
                },
                "Gain musculaire" => new List<string>
                {
                    "🍗 200g poulet",
                    "🍚 100g riz",
                    "🥦 Légumes grillés",
                    "🥗 Salade"
                },
                _ => new List<string>
                {
                    "🥘 Tajine de légumes",
                    "🌾 Couscous complet",
                    "🥗 Salade marocaine",
                    "🍵 Thé à la menthe"
                }
            };
        }

        public List<WeeklyWorkoutPlan> GenerateWeeklyWorkoutPlan(ApplicationUser user)
        {
            var workoutsPerWeek = user.WorkoutsPerWeek;
            var fitnessGoal = user.FitnessGoal;
            
            return GenerateWorkoutSchedule(workoutsPerWeek, fitnessGoal);
        }

        private List<WeeklyWorkoutPlan> GenerateWorkoutSchedule(int workoutsPerWeek, string fitnessGoal)
        {
            var days = new[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
            var weeklyPlan = new List<WeeklyWorkoutPlan>();

            if (workoutsPerWeek >= 5)
            {
                // Programme intensif - Push/Pull/Legs + Cardio
                weeklyPlan.Add(CreateWorkout("Lundi", "Musculation - Poussée", GetPushExercises(), 60));
                weeklyPlan.Add(CreateWorkout("Mardi", "Musculation - Tirage", GetPullExercises(), 60));
                weeklyPlan.Add(CreateWorkout("Mercredi", "Cardio / HIIT", GetCardioExercises(), 40));
                weeklyPlan.Add(CreateWorkout("Jeudi", "Musculation - Jambes", GetLegExercises(), 60));
                weeklyPlan.Add(CreateWorkout("Vendredi", "Musculation - Full Body", GetFullBodyExercises(), 60));
                weeklyPlan.Add(CreateWorkout("Samedi", "Cardio Léger", GetLightCardio(), 30));
                weeklyPlan.Add(CreateWorkout("Dimanche", "Repos", new List<Exercise>(), 0));
            }
            else if (workoutsPerWeek >= 3)
            {
                // Programme modéré - Full Body
                weeklyPlan.Add(CreateWorkout("Lundi", "Musculation - Full Body", GetFullBodyExercises(), 60));
                weeklyPlan.Add(CreateWorkout("Mardi", "Repos", new List<Exercise>(), 0));
                weeklyPlan.Add(CreateWorkout("Mercredi", "Cardio / HIIT", GetCardioExercises(), 40));
                weeklyPlan.Add(CreateWorkout("Jeudi", "Repos", new List<Exercise>(), 0));
                weeklyPlan.Add(CreateWorkout("Vendredi", "Musculation - Full Body", GetFullBodyExercises(), 60));
                weeklyPlan.Add(CreateWorkout("Samedi", "Cardio Léger", GetLightCardio(), 30));
                weeklyPlan.Add(CreateWorkout("Dimanche", "Repos", new List<Exercise>(), 0));
            }
            else
            {
                // Programme débutant
                weeklyPlan.Add(CreateWorkout("Lundi", "Musculation - Corps Complet", GetBeginnerExercises(), 45));
                weeklyPlan.Add(CreateWorkout("Mardi", "Repos", new List<Exercise>(), 0));
                weeklyPlan.Add(CreateWorkout("Mercredi", "Repos", new List<Exercise>(), 0));
                weeklyPlan.Add(CreateWorkout("Jeudi", "Cardio Modéré", GetLightCardio(), 30));
                weeklyPlan.Add(CreateWorkout("Vendredi", "Repos", new List<Exercise>(), 0));
                weeklyPlan.Add(CreateWorkout("Samedi", "Musculation - Corps Complet", GetBeginnerExercises(), 45));
                weeklyPlan.Add(CreateWorkout("Dimanche", "Repos", new List<Exercise>(), 0));
            }

            return weeklyPlan;
        }

        private WeeklyWorkoutPlan CreateWorkout(string day, string type, List<Exercise> exercises, int duration)
        {
            return new WeeklyWorkoutPlan
            {
                Day = day,
                WorkoutType = type,
                Exercises = exercises,
                DurationMinutes = duration
            };
        }

        private List<Exercise> GetPushExercises()
        {
            return new List<Exercise>
            {
                new Exercise { Name = "Développé Couché", Sets = "4x8-10", Description = "Pectoraux", Emoji = "💪" },
                new Exercise { Name = "Développé Incliné", Sets = "3x10-12", Description = "Haut des pecs", Emoji = "🏋️" },
                new Exercise { Name = "Dips", Sets = "3x12-15", Description = "Triceps et pecs", Emoji = "💪" },
                new Exercise { Name = "Développé Militaire", Sets = "4x8-10", Description = "Épaules", Emoji = "🏋️" },
                new Exercise { Name = "Élévations Latérales", Sets = "3x12-15", Description = "Épaules latérales", Emoji = "💪" },
                new Exercise { Name = "Extensions Triceps", Sets = "3x12-15", Description = "Triceps", Emoji = "💪" }
            };
        }

        private List<Exercise> GetPullExercises()
        {
            return new List<Exercise>
            {
                new Exercise { Name = "Tractions", Sets = "4x8-10", Description = "Dos et biceps", Emoji = "💪" },
                new Exercise { Name = "Rowing Barre", Sets = "4x8-10", Description = "Dos épais", Emoji = "🏋️" },
                new Exercise { Name = "Tirage Vertical", Sets = "3x10-12", Description = "Dos large", Emoji = "💪" },
                new Exercise { Name = "Rowing Haltères", Sets = "3x10-12", Description = "Dos", Emoji = "🏋️" },
                new Exercise { Name = "Curl Biceps", Sets = "3x12-15", Description = "Biceps", Emoji = "💪" },
                new Exercise { Name = "Curl Marteau", Sets = "3x12-15", Description = "Avant-bras", Emoji = "💪" }
            };
        }

        private List<Exercise> GetLegExercises()
        {
            return new List<Exercise>
            {
                new Exercise { Name = "Squat", Sets = "4x8-10", Description = "Cuisses complètes", Emoji = "🦵" },
                new Exercise { Name = "Presse à Cuisses", Sets = "4x10-12", Description = "Quadriceps", Emoji = "🏋️" },
                new Exercise { Name = "Soulevé de Terre Roumain", Sets = "3x10-12", Description = "Ischios", Emoji = "💪" },
                new Exercise { Name = "Leg Curl", Sets = "3x12-15", Description = "Ischios", Emoji = "🦵" },
                new Exercise { Name = "Extensions Mollets", Sets = "4x15-20", Description = "Mollets", Emoji = "🦵" },
                new Exercise { Name = "Abdo Crunch", Sets = "3x20", Description = "Abdominaux", Emoji = "💪" }
            };
        }

        private List<Exercise> GetFullBodyExercises()
        {
            return new List<Exercise>
            {
                new Exercise { Name = "Squat", Sets = "3x10", Description = "Jambes", Emoji = "🦵" },
                new Exercise { Name = "Développé Couché", Sets = "3x10", Description = "Pectoraux", Emoji = "💪" },
                new Exercise { Name = "Rowing", Sets = "3x10", Description = "Dos", Emoji = "🏋️" },
                new Exercise { Name = "Développé Militaire", Sets = "3x10", Description = "Épaules", Emoji = "💪" },
                new Exercise { Name = "Soulevé de Terre", Sets = "3x8", Description = "Corps complet", Emoji = "🏋️" },
                new Exercise { Name = "Abdos", Sets = "3x15", Description = "Core", Emoji = "💪" }
            };
        }

        private List<Exercise> GetCardioExercises()
        {
            return new List<Exercise>
            {
                new Exercise { Name = "Échauffement", Sets = "5 min", Description = "Vélo ou marche", Emoji = "🚴" },
                new Exercise { Name = "HIIT Sprint", Sets = "8x30s", Description = "30s sprint / 60s repos", Emoji = "🏃" },
                new Exercise { Name = "Burpees", Sets = "3x15", Description = "Cardio intense", Emoji = "💥" },
                new Exercise { Name = "Mountain Climbers", Sets = "3x30s", Description = "Core et cardio", Emoji = "⛰️" },
                new Exercise { Name = "Jump Rope", Sets = "5 min", Description = "Corde à sauter", Emoji = "🪢" },
                new Exercise { Name = "Retour au calme", Sets = "5 min", Description = "Marche lente", Emoji = "🚶" }
            };
        }

        private List<Exercise> GetLightCardio()
        {
            return new List<Exercise>
            {
                new Exercise { Name = "Marche Rapide", Sets = "20 min", Description = "Rythme soutenu", Emoji = "🚶" },
                new Exercise { Name = "Vélo", Sets = "10 min", Description = "Intensité modérée", Emoji = "🚴" },
                new Exercise { Name = "Étirements", Sets = "10 min", Description = "Flexibilité", Emoji = "🧘" }
            };
        }

        private List<Exercise> GetBeginnerExercises()
        {
            return new List<Exercise>
            {
                new Exercise { Name = "Squat au poids du corps", Sets = "3x12", Description = "Jambes", Emoji = "🦵" },
                new Exercise { Name = "Pompes", Sets = "3x10", Description = "Pectoraux", Emoji = "💪" },
                new Exercise { Name = "Rowing avec haltères", Sets = "3x12", Description = "Dos", Emoji = "🏋️" },
                new Exercise { Name = "Fentes", Sets = "3x10/jambe", Description = "Jambes", Emoji = "🦵" },
                new Exercise { Name = "Planche", Sets = "3x30s", Description = "Core", Emoji = "💪" },
                new Exercise { Name = "Crunch", Sets = "3x15", Description = "Abdos", Emoji = "💪" }
            };
        }
    }
}
