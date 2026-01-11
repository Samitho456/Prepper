using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Prepper.Models
{
    [Table("meal_plans")]
    public class MealPlan : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("created_at", ignoreOnInsert: true)]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("is_consumed")]
        public bool IsConsumed { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        [Column("recipe_id")]
        public int RecipeId { get; set; }
        [Column("meal_type")]
        public string MealType { get; set; } // e.g., Breakfast, Lunch, Dinner

        [Column("date")]
        public DateOnly Date { get; set; }
        // Parameterized constructor
        public MealPlan(int id, DateTimeOffset createdAt, bool isConsumed, int userId, int recipeId, string mealType)
        {
            Id = id;
            CreatedAt = createdAt;
            IsConsumed = isConsumed;
            UserId = userId;
            RecipeId = recipeId;
            MealType = mealType;
        }

        // Default constructor
        public MealPlan() { }

        // Override ToString for better readability
        public override string ToString()
        {
            return $"Id: {Id}, Created at: {CreatedAt}, IsConsumed: {IsConsumed}, UserId: {UserId}, RecipeId: {RecipeId}, MealType: {MealType}";
        }
    }
}
