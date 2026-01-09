using Prepper.DTOs;

namespace UnitTests.DTOTests
{
    [TestClass]
    public class RecipeInstructionDTOTests
    {
        [TestMethod]
        public void CreateRecipeInstructionDTO_WithDefaultConstructor()
        {
            // Act
            var dto = new RecipeInstructionDTO();

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Id);
            Assert.AreEqual(0, dto.RecipeId);
            Assert.AreEqual(0, dto.StepNumber);
            Assert.IsNull(dto.InstructionText);
        }

        [TestMethod]
        public void RecipeInstructionDTO_SetAllProperties_Success()
        {
            // Arrange
            var dto = new RecipeInstructionDTO();
            var createdAt = DateTimeOffset.Now;

            // Act
            dto.Id = 1;
            dto.CreatedAt = createdAt;
            dto.RecipeId = 10;
            dto.StepNumber = 1;
            dto.InstructionText = "Preheat the oven to 350°F";

            // Assert
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.AreEqual(10, dto.RecipeId);
            Assert.AreEqual(1, dto.StepNumber);
            Assert.AreEqual("Preheat the oven to 350°F", dto.InstructionText);
        }

        [TestMethod]
        public void RecipeInstructionDTO_InitializeWithObjectInitializer()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);

            // Act
            var dto = new RecipeInstructionDTO
            {
                Id = 5,
                CreatedAt = createdAt,
                RecipeId = 20,
                StepNumber = 3,
                InstructionText = "Mix all ingredients together"
            };

            // Assert
            Assert.AreEqual(5, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.AreEqual(20, dto.RecipeId);
            Assert.AreEqual(3, dto.StepNumber);
            Assert.AreEqual("Mix all ingredients together", dto.InstructionText);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithNullInstructionText()
        {
            // Act
            var dto = new RecipeInstructionDTO { InstructionText = null };

            // Assert
            Assert.IsNull(dto.InstructionText);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithEmptyInstructionText()
        {
            // Act
            var dto = new RecipeInstructionDTO { InstructionText = string.Empty };

            // Assert
            Assert.AreEqual(string.Empty, dto.InstructionText);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithWhitespaceInstructionText()
        {
            // Act
            var dto = new RecipeInstructionDTO { InstructionText = "   " };

            // Assert
            Assert.AreEqual("   ", dto.InstructionText);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithLongInstructionText()
        {
            // Arrange
            var longText = new string('A', 2000);

            // Act
            var dto = new RecipeInstructionDTO { InstructionText = longText };

            // Assert
            Assert.AreEqual(longText, dto.InstructionText);
            Assert.AreEqual(2000, dto.InstructionText.Length);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithZeroStepNumber()
        {
            // Act
            var dto = new RecipeInstructionDTO { StepNumber = 0 };

            // Assert
            Assert.AreEqual(0, dto.StepNumber);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithNegativeStepNumber()
        {
            // Act
            var dto = new RecipeInstructionDTO { StepNumber = -1 };

            // Assert
            Assert.AreEqual(-1, dto.StepNumber);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithLargeStepNumber()
        {
            // Act
            var dto = new RecipeInstructionDTO { StepNumber = 999 };

            // Assert
            Assert.AreEqual(999, dto.StepNumber);
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithMultilineInstructionText()
        {
            // Arrange
            var multilineText = "Step 1: First action\nStep 2: Second action\nStep 3: Third action";

            // Act
            var dto = new RecipeInstructionDTO { InstructionText = multilineText };

            // Assert
            Assert.AreEqual(multilineText, dto.InstructionText);
            Assert.IsTrue(dto.InstructionText.Contains("\n"));
        }

        [TestMethod]
        public void RecipeInstructionDTO_WithSpecialCharacters()
        {
            // Arrange
            var specialText = "Add 1/2 cup of sugar & stir @ 180°C for 5-10 minutes";

            // Act
            var dto = new RecipeInstructionDTO { InstructionText = specialText };

            // Assert
            Assert.AreEqual(specialText, dto.InstructionText);
        }

        [TestMethod]
        public void RecipeInstructionDTO_CreatedAtDefaultValue()
        {
            // Act
            var dto = new RecipeInstructionDTO();

            // Assert
            Assert.AreEqual(DateTimeOffset.MinValue, dto.CreatedAt);
        }

        [TestMethod]
        public void RecipeInstructionDTO_MultipleStepsForSameRecipe()
        {
            // Act
            var step1 = new RecipeInstructionDTO { RecipeId = 1, StepNumber = 1 };
            var step2 = new RecipeInstructionDTO { RecipeId = 1, StepNumber = 2 };
            var step3 = new RecipeInstructionDTO { RecipeId = 1, StepNumber = 3 };

            // Assert
            Assert.AreEqual(1, step1.RecipeId);
            Assert.AreEqual(1, step2.RecipeId);
            Assert.AreEqual(1, step3.RecipeId);
            Assert.AreEqual(1, step1.StepNumber);
            Assert.AreEqual(2, step2.StepNumber);
            Assert.AreEqual(3, step3.StepNumber);
        }
    }
}
