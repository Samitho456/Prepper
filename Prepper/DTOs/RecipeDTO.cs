using System.ComponentModel.DataAnnotations;

namespace Prepper.DTOs
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int Servings { get; set; }

        [Required]
        [StringLength(100)]
        public string MealType { get; set; }

        [Range(0, int.MaxValue)]
        public int PreparationTimeMinutes { get; set; }
    }
}
