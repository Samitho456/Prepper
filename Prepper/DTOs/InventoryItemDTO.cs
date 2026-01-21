using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace Prepper.DTOs
{
    public record InventoryItemDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int? IngredientId { get; set; }
        public int? RecipeId { get; set; }
        public float Quantity { get; set; }
        public string Unit { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }
    }
}
