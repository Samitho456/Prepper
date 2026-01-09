using System.ComponentModel.DataAnnotations;

namespace Prepper.DTOs
{
    public class RecipeIngredientDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "RecipeId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "RecipeId must be greater than 0")]
        public int RecipeId { get; set; }
        
        [Required(ErrorMessage = "IngredientId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "IngredientId must be greater than 0")]
        public int IngredientId { get; set; }
        
        [Required(ErrorMessage = "Quantity is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Quantity must be between 1 and 100 characters")]
        public string Quantity { get; set; }
        
        [Required(ErrorMessage = "Unit is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Unit must be between 1 and 50 characters")]
        public string Unit { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
    }
}
