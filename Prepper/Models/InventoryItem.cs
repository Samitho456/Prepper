using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Prepper.Models
{
    [Table("inventory_items")]
    public class InventoryItem : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        [Column("created_at", ignoreOnInsert: true)]
        public DateTimeOffset CreatedAt { get; set; }
        [Column("ingredient_id")]
        public int? IngredientId { get; set; }
        [Column("recipe_id")]
        public int? RecipeId { get; set; }
        [Column("quantity")]
        public float Quantity { get; set; }
        [Column("unit")]
        public string Unit { get; set; }
        [Column("expiration_date")]
        public DateTimeOffset? ExpirationDate { get; set; }
        [Column("location_id")]
        public int LocationId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }

        public InventoryItem(int id, DateTimeOffset createdAt, int? ingredientId, int? recipeId, float quantity, string unit, DateTimeOffset? expirationDate, int locationId, int userId)
        {
            Id = id;
            CreatedAt = createdAt;
            IngredientId = ingredientId;
            RecipeId = recipeId;
            Quantity = quantity;
            Unit = unit;
            ExpirationDate = expirationDate;
            LocationId = locationId;
            UserId = userId;
        }

        public InventoryItem() { }

        public override string ToString()
        {
            return $"Id: {Id}, Created at: {CreatedAt}, IngredientId: {IngredientId}, RecipeId: {RecipeId}, Quantity: {Quantity}, Unit: {Unit}, ExpirationDate: {ExpirationDate}, LocationId: {LocationId}, UserId: {UserId}";
        }
    }
}
