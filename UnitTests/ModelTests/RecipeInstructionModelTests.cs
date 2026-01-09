using Prepper.Models;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class RecipeInstructionModelTests
    {
        [TestMethod]
        public void CreateValidRecipeInstruction_WithParameterizedConstructor()
        {
            // Arrange
            var id = 1;
            var createdAt = DateTimeOffset.Now;
            var recipeId = 10;
            var stepNumber = 1;
            var instructionText = "Preheat oven to 350°F";

            // Act
            var instruction = new RecipeInstruction(id, createdAt, recipeId, stepNumber, instructionText);

            // Assert
            Assert.AreEqual(id, instruction.Id);
            Assert.AreEqual(createdAt, instruction.CreatedAt);
            Assert.AreEqual(recipeId, instruction.RecipeId);
            Assert.AreEqual(stepNumber, instruction.StepNumber);
            Assert.AreEqual(instructionText, instruction.InstructionText);
        }

        [TestMethod]
        public void CreateValidRecipeInstruction_WithDefaultConstructor()
        {
            // Act
            var instruction = new RecipeInstruction();

            // Assert
            Assert.IsNotNull(instruction);
            Assert.AreEqual(0, instruction.Id);
            Assert.AreEqual(0, instruction.RecipeId);
            Assert.AreEqual(0, instruction.StepNumber);
            Assert.IsNull(instruction.InstructionText);
        }

        [TestMethod]
        public void RecipeInstruction_SetProperties_Success()
        {
            // Arrange
            var instruction = new RecipeInstruction();
            var createdAt = DateTimeOffset.Now;

            // Act
            instruction.Id = 5;
            instruction.CreatedAt = createdAt;
            instruction.RecipeId = 20;
            instruction.StepNumber = 3;
            instruction.InstructionText = "Mix ingredients thoroughly";

            // Assert
            Assert.AreEqual(5, instruction.Id);
            Assert.AreEqual(createdAt, instruction.CreatedAt);
            Assert.AreEqual(20, instruction.RecipeId);
            Assert.AreEqual(3, instruction.StepNumber);
            Assert.AreEqual("Mix ingredients thoroughly", instruction.InstructionText);
        }

        [TestMethod]
        public void RecipeInstruction_ToString_ReturnsFormattedString()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
            var instruction = new RecipeInstruction(1, createdAt, 5, 2, "Stir well");

            // Act
            var result = instruction.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Id: 1"));
            Assert.IsTrue(result.Contains("RecipeId: 5"));
            Assert.IsTrue(result.Contains("StepNumber: 2"));
            Assert.IsTrue(result.Contains("InstructionText: Stir well"));
        }

        [TestMethod]
        public void RecipeInstruction_WithNullInstructionText()
        {
            // Act
            var instruction = new RecipeInstruction(1, DateTimeOffset.Now, 1, 1, null);

            // Assert
            Assert.IsNull(instruction.InstructionText);
        }

        [TestMethod]
        public void RecipeInstruction_WithEmptyInstructionText()
        {
            // Act
            var instruction = new RecipeInstruction(1, DateTimeOffset.Now, 1, 1, string.Empty);

            // Assert
            Assert.AreEqual(string.Empty, instruction.InstructionText);
        }

        [TestMethod]
        public void RecipeInstruction_WithZeroStepNumber()
        {
            // Act
            var instruction = new RecipeInstruction(1, DateTimeOffset.Now, 1, 0, "Test");

            // Assert
            Assert.AreEqual(0, instruction.StepNumber);
        }

        [TestMethod]
        public void RecipeInstruction_WithNegativeStepNumber()
        {
            // Act
            var instruction = new RecipeInstruction(1, DateTimeOffset.Now, 1, -1, "Test");

            // Assert
            Assert.AreEqual(-1, instruction.StepNumber);
        }

        [TestMethod]
        public void RecipeInstruction_WithLargeStepNumber()
        {
            // Act
            var instruction = new RecipeInstruction(1, DateTimeOffset.Now, 1, 999, "Final step");

            // Assert
            Assert.AreEqual(999, instruction.StepNumber);
        }
    }
}
