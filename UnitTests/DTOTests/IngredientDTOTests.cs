using Prepper.DTOs;
using System.ComponentModel.DataAnnotations;

namespace UnitTests.DTOTests
{
    [TestClass]
    public class IngredientDTOTests
    {
        [TestMethod]
        public void CreateIngredientDTO_WithDefaultConstructor()
        {
            // Act
            var dto = new IngredientDTO();

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Id);
            Assert.IsNull(dto.Name);
            Assert.IsNull(dto.CreatedAt);
        }

        [TestMethod]
        public void IngredientDTO_SetAllProperties_Success()
        {
            // Arrange
            var dto = new IngredientDTO();
            var createdAt = DateTimeOffset.Now;

            // Act
            dto.Id = 1;
            dto.Name = "Tomato";
            dto.CreatedAt = createdAt;

            // Assert
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual("Tomato", dto.Name);
            Assert.AreEqual(createdAt, dto.CreatedAt);
        }

        [TestMethod]
        public void IngredientDTO_InitializeWithObjectInitializer()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);

            // Act
            var dto = new IngredientDTO
            {
                Id = 5,
                Name = "Garlic",
                CreatedAt = createdAt
            };

            // Assert
            Assert.AreEqual(5, dto.Id);
            Assert.AreEqual("Garlic", dto.Name);
            Assert.AreEqual(createdAt, dto.CreatedAt);
        }

        [TestMethod]
        public void IngredientDTO_WithNullName()
        {
            // Act
            var dto = new IngredientDTO { Name = null };

            // Assert
            Assert.IsNull(dto.Name);
        }

        [TestMethod]
        public void IngredientDTO_WithEmptyName()
        {
            // Act
            var dto = new IngredientDTO { Name = string.Empty };

            // Assert
            Assert.AreEqual(string.Empty, dto.Name);
        }

        [TestMethod]
        public void IngredientDTO_WithMinimumValidNameLength()
        {
            // Act
            var dto = new IngredientDTO { Name = "AB" };

            // Assert
            Assert.AreEqual("AB", dto.Name);
            Assert.AreEqual(2, dto.Name.Length);
        }

        [TestMethod]
        public void IngredientDTO_WithMaximumValidNameLength()
        {
            // Arrange
            var maxLengthName = new string('A', 100);

            // Act
            var dto = new IngredientDTO { Name = maxLengthName };

            // Assert
            Assert.AreEqual(maxLengthName, dto.Name);
            Assert.AreEqual(100, dto.Name.Length);
        }

        [TestMethod]
        public void IngredientDTO_WithSingleCharacterName()
        {
            // Act
            var dto = new IngredientDTO { Name = "A" };

            // Assert
            Assert.AreEqual("A", dto.Name);
            Assert.AreEqual(1, dto.Name.Length);
        }

        [TestMethod]
        public void IngredientDTO_WithNameExceedingMaxLength()
        {
            // Arrange
            var tooLongName = new string('A', 101);

            // Act
            var dto = new IngredientDTO { Name = tooLongName };

            // Assert
            Assert.AreEqual(tooLongName, dto.Name);
            Assert.AreEqual(101, dto.Name.Length);
        }

        [TestMethod]
        public void IngredientDTO_WithNullCreatedAt()
        {
            // Act
            var dto = new IngredientDTO { CreatedAt = null };

            // Assert
            Assert.IsNull(dto.CreatedAt);
        }

        [TestMethod]
        public void IngredientDTO_CreatedAtDefaultValue()
        {
            // Act
            var dto = new IngredientDTO();

            // Assert
            Assert.IsNull(dto.CreatedAt);
        }

        [TestMethod]
        public void IngredientDTO_WithWhitespaceName()
        {
            // Act
            var dto = new IngredientDTO { Name = "   " };

            // Assert
            Assert.AreEqual("   ", dto.Name);
        }

        [TestMethod]
        public void IngredientDTO_WithNameContainingSpaces()
        {
            // Act
            var dto = new IngredientDTO { Name = "Red Bell Pepper" };

            // Assert
            Assert.AreEqual("Red Bell Pepper", dto.Name);
        }

        [TestMethod]
        public void IngredientDTO_WithNameContainingSpecialCharacters()
        {
            // Act
            var dto = new IngredientDTO { Name = "Jalapeño Pepper" };

            // Assert
            Assert.AreEqual("Jalapeño Pepper", dto.Name);
        }

        [TestMethod]
        public void IngredientDTO_WithNameContainingNumbers()
        {
            // Act
            var dto = new IngredientDTO { Name = "Type 00 Flour" };

            // Assert
            Assert.AreEqual("Type 00 Flour", dto.Name);
        }

        [TestMethod]
        public void IngredientDTO_ValidateRequiredAttribute()
        {
            // Arrange
            var dto = new IngredientDTO { Id = 1, CreatedAt = DateTimeOffset.Now };
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains("Name")));
        }

        [TestMethod]
        public void IngredientDTO_ValidateStringLengthAttribute_TooShort()
        {
            // Arrange
            var dto = new IngredientDTO { Id = 1, Name = "A", CreatedAt = DateTimeOffset.Now };
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains("Name")));
        }

        [TestMethod]
        public void IngredientDTO_ValidateStringLengthAttribute_TooLong()
        {
            // Arrange
            var tooLongName = new string('A', 101);
            var dto = new IngredientDTO { Id = 1, Name = tooLongName, CreatedAt = DateTimeOffset.Now };
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains("Name")));
        }

        [TestMethod]
        public void IngredientDTO_ValidateValidObject()
        {
            // Arrange
            var dto = new IngredientDTO { Id = 1, Name = "Valid Ingredient", CreatedAt = DateTimeOffset.Now };
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void IngredientDTO_WithCommonIngredientNames()
        {
            // Act & Assert
            var salt = new IngredientDTO { Name = "Salt" };
            Assert.AreEqual("Salt", salt.Name);

            var pepper = new IngredientDTO { Name = "Black Pepper" };
            Assert.AreEqual("Black Pepper", pepper.Name);

            var olive = new IngredientDTO { Name = "Extra Virgin Olive Oil" };
            Assert.AreEqual("Extra Virgin Olive Oil", olive.Name);

            var flour = new IngredientDTO { Name = "All-Purpose Flour" };
            Assert.AreEqual("All-Purpose Flour", flour.Name);
        }
    }
}
