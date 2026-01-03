using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.Models
{
    public class Recipe : BaseModel
    {
        // Id of the Recipe
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        // Title of the Recipe
        [Column("title")]
        public string Title { get; set; }

        // Description of the Recipe
        [Column("description")]
        public string Description { get; set; }

        // Amount of servigs the Recipe make up
        [Column("servings")]
        public int Servings { get; set; }

        // Minutes is take to prepare the meal
        [Column("preparation_minutes")]
        public int PreparationTimeMinutes { get; set; }

        // The type of meal (e. g., Breakfast, Lunch, Dinner, Desert, etc.)
        [Column("meal_type")]
        public string MealType { get; set; }

        // Time of creation in the database
        [Column("created_at", ignoreOnInsert: true)]
        public DateTimeOffset CreatedAt { get; set; }

        // Parameterized constructor
        public Recipe(int id, string title, string description, int servings, string mealType, DateTimeOffset createdAt)
        {
            Id = id;
            Title = title;
            Description = description;
            Servings = servings;
            MealType = mealType;
            CreatedAt = createdAt;
        }

        // Default Constructor
        public Recipe() { }

        // Override ToString for better readability
        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Servings: {Servings}, Meal type: {MealType}, Description: {Description}, Created at {CreatedAt}";
        }
    }
}
