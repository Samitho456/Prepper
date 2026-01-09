namespace Prepper.DTOs
{
    public class RecipeInstructionDTO
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string InstructionText { get; set; }
    }
}
