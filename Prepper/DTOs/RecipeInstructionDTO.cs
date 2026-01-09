using System.ComponentModel.DataAnnotations;

namespace Prepper.DTOs
{
    public class RecipeInstructionDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        [Range(1, int.MaxValue)]
        public int RecipeId { get; set; }

        [Range(1, int.MaxValue)]
        public int StepNumber { get; set; }

        [Required]
        [StringLength(1000)]
        public string InstructionText { get; set; }
    }
}
