using Prepper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.DTOs
{
    public class CompleteRecipeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Servings { get; set; }
        public int PreparationTimeMinutes { get; set; }
        public string MealType { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<CompleteRecipeIngredientDTO> Ingredients { get; set; }
        public List<RecipeInstructionDTO> Instructions { get; set; }
        public NutritionalProfileDTO NutritionalProfilePerServing { get; set; }
    }
}
