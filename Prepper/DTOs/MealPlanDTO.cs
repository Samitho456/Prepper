using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prepper.DTOs
{
    public class MealPlanDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsConsumed { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RecipeId { get; set; }
        public string MealType { get; set; } // e.g., Breakfast, Lunch, Dinner'
        [Required]
        public DateOnly Date { get; set; }
    }
}
