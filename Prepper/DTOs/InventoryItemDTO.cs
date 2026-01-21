using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace Prepper.DTOs
{
    public record InventoryItemDTO
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [Range(0, int.MaxValue)]
        public int IngredientId { get; set; }
        [Range(0, int.MaxValue)]
        public int RecipeId { get; set; }
        [Required]
        public float Quantity { get; set; }
        [Required]
        public string Unit { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int LocationId { get; set; }
        [Range(0, int.MaxValue)]
        public int UserId { get; set; }
    }
}
