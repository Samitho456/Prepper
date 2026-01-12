using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.DTOs
{
    public class CompleteRecipeIngredientDTO
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
