using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prepper.DTOs
{
    public class MealPlanWithRecipeDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsConsumed { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RecipeId { get; set; }
        public string MealType { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        public CompleteRecipeDTO Recipe { get; set; }
    }
}
