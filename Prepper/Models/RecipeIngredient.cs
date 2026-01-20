using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Prepper.Models
{
    [Table("recipe_ingredients")]
    public class RecipeIngredient : BaseModel
    {
        // Unique identifier for the recipe ingredient entry
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        // Foreign key to the associated recipe
        [Column("recipe_id")]
        public int RecipeId { get; set; }

        // Foreign key to the associated ingredient
        [Column("ingredient_id")]
        public int IngredientId { get; set; }

        // Quantity of the ingredient needed for the recipe
        [Column("quantity")]
        public double Quantity { get; set; }

        // Unit of measurement for the ingredient quantity (e.g., grams, cups)
        [Column("unit")]
        public string Unit { get; set; }

        // Timestamp of when the recipe ingredient entry was created
        [Column("created_at", ignoreOnInsert: true)]
        public DateTimeOffset CreatedAt { get; set; }

        // Parameterized constructor
        public RecipeIngredient(int id, int recipeId, int ingredientId, double quantity, string unit, DateTimeOffset createdAt)
        {
            Id = id;
            RecipeId = recipeId;
            IngredientId = ingredientId;
            Quantity = quantity;
            Unit = unit;
            CreatedAt = createdAt;
        }

        // Default constructor
        public RecipeIngredient() { }

        // Override ToString for better readability
        public override string ToString()
        {
            return $"Id: {Id}, RecipeId: {RecipeId}, IngredientId: {IngredientId}, Quantity: {Quantity} {Unit}, Created at {CreatedAt}";
        }
    }
}
