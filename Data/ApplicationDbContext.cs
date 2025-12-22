using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FitTrack.Models;

namespace FitTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FoodLibrary> FoodLibraries { get; set; }
        public DbSet<MealLog> MealLogs { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<HydrationLog> HydrationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed 20 essential food items with Moroccan touches
            modelBuilder.Entity<FoodLibrary>().HasData(
                new FoodLibrary { Id = 1, Name = "Poulet (Poitrine)", CaloriesPer100g = 165, ProteinPer100g = 31, CarbsPer100g = 0, FatPer100g = 3.6m, Category = "Protéine" },
                new FoodLibrary { Id = 2, Name = "Riz Brun", CaloriesPer100g = 111, ProteinPer100g = 2.6m, CarbsPer100g = 23, FatPer100g = 0.9m, Category = "Céréales" },
                new FoodLibrary { Id = 3, Name = "Œufs", CaloriesPer100g = 155, ProteinPer100g = 13, CarbsPer100g = 1.1m, FatPer100g = 11, Category = "Protéine" },
                new FoodLibrary { Id = 4, Name = "Whey Protéine", CaloriesPer100g = 400, ProteinPer100g = 80, CarbsPer100g = 8, FatPer100g = 5, Category = "Supplément" },
                new FoodLibrary { Id = 5, Name = "Saumon", CaloriesPer100g = 208, ProteinPer100g = 20, CarbsPer100g = 0, FatPer100g = 13, Category = "Protéine" },
                new FoodLibrary { Id = 6, Name = "Patate Douce", CaloriesPer100g = 86, ProteinPer100g = 1.6m, CarbsPer100g = 20, FatPer100g = 0.1m, Category = "Légumes" },
                new FoodLibrary { Id = 7, Name = "Quinoa", CaloriesPer100g = 120, ProteinPer100g = 4.4m, CarbsPer100g = 21, FatPer100g = 1.9m, Category = "Céréales" },
                new FoodLibrary { Id = 8, Name = "Bœuf Haché Maigre", CaloriesPer100g = 250, ProteinPer100g = 26, CarbsPer100g = 0, FatPer100g = 15, Category = "Protéine" },
                new FoodLibrary { Id = 9, Name = "Avocat", CaloriesPer100g = 160, ProteinPer100g = 2, CarbsPer100g = 9, FatPer100g = 15, Category = "Fruits" },
                new FoodLibrary { Id = 10, Name = "Amandes", CaloriesPer100g = 579, ProteinPer100g = 21, CarbsPer100g = 22, FatPer100g = 50, Category = "Noix" },
                new FoodLibrary { Id = 11, Name = "Couscous", CaloriesPer100g = 112, ProteinPer100g = 3.8m, CarbsPer100g = 23, FatPer100g = 0.2m, Category = "Céréales" },
                new FoodLibrary { Id = 12, Name = "Lentilles", CaloriesPer100g = 116, ProteinPer100g = 9, CarbsPer100g = 20, FatPer100g = 0.4m, Category = "Légumineuses" },
                new FoodLibrary { Id = 13, Name = "Yaourt Grec", CaloriesPer100g = 59, ProteinPer100g = 10, CarbsPer100g = 3.6m, FatPer100g = 0.4m, Category = "Produits Laitiers" },
                new FoodLibrary { Id = 14, Name = "Banane", CaloriesPer100g = 89, ProteinPer100g = 1.1m, CarbsPer100g = 23, FatPer100g = 0.3m, Category = "Fruits" },
                new FoodLibrary { Id = 15, Name = "Brocoli", CaloriesPer100g = 34, ProteinPer100g = 2.8m, CarbsPer100g = 7, FatPer100g = 0.4m, Category = "Légumes" },
                new FoodLibrary { Id = 16, Name = "Thon en Conserve", CaloriesPer100g = 116, ProteinPer100g = 26, CarbsPer100g = 0, FatPer100g = 0.8m, Category = "Protéine" },
                new FoodLibrary { Id = 17, Name = "Huile d'Olive", CaloriesPer100g = 884, ProteinPer100g = 0, CarbsPer100g = 0, FatPer100g = 100, Category = "Graisses" },
                new FoodLibrary { Id = 18, Name = "Épinards", CaloriesPer100g = 23, ProteinPer100g = 2.9m, CarbsPer100g = 3.6m, FatPer100g = 0.4m, Category = "Légumes" },
                new FoodLibrary { Id = 19, Name = "Pain Complet", CaloriesPer100g = 247, ProteinPer100g = 13, CarbsPer100g = 41, FatPer100g = 3.4m, Category = "Céréales" },
                new FoodLibrary { Id = 20, Name = "Dattes Medjool", CaloriesPer100g = 277, ProteinPer100g = 1.8m, CarbsPer100g = 75, FatPer100g = 0.2m, Category = "Fruits" }
            );

            // Add sample objectives
            // modelBuilder.Entity<Objective>().HasData(
            //     new Objective 
            //     { 
            //         Id = 1, 
            //         UserId = "sample-user-id",
            //         Title = "Perdre 5kg", 
            //         Type = "Poids", 
            //         GoalValue = 75, 
            //         StartDate = DateTime.Now.AddDays(-30), 
            //         TargetDate = DateTime.Now.AddDays(60),
            //         IsAchieved = false
            //     }
            // );

            // Add sample measure
            // modelBuilder.Entity<Measure>().HasData(
            //     new Measure 
            //     { 
            //         Id = 1,
            //         UserId = "sample-user-id", 
            //         Date = DateTime.Now, 
            //         Weight = 80, 
            //         BodyFatPercentage = 20, 
            //         MuscleMass = 35,
            //         Notes = "Mesure initiale"
            //     }
            // );
        }
    }
}
