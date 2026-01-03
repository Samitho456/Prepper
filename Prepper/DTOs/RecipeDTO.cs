using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.DTOs
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Servings { get; set; }
        public string MealType { get; set; }
        public int PreparationTimeMinutes { get; set; }
    }
}
