using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prepper.DTOs
{
    public class CompleteRecipeIngredientDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "RecepeId is required")]
        public int RecipeId { get; set; }
        [Required(ErrorMessage = "IngredientId is required")]
        public int IngredientId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0")]
        public double Quantity { get; set; }
        [Required(ErrorMessage = "Unit is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Unit must be between 1 and 50 characters")]
        public string Unit { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
