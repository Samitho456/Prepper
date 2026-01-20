using Prepper.DTOs;

namespace UnitTests.DTOTests
{
    [TestClass]
    public class RecipeIngredientDTOTests
    {
        [TestMethod]
        public void CreateRecipeIngredientDTO_WithDefaultConstructor()
        {
            // Act
            var dto = new RecipeIngredientDTO();

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Id);
            Assert.AreEqual(0, dto.RecipeId);
            Assert.AreEqual(0, dto.IngredientId);
            Assert.AreEqual(0.0, dto.Quantity);
            Assert.IsNull(dto.Unit);
        }

        [TestMethod]
        public void RecipeIngredientDTO_SetAllProperties_Success()
        {
            // Arrange
            var dto = new RecipeIngredientDTO();
            var createdAt = DateTimeOffset.Now;

            // Act
            dto.Id = 1;
            dto.RecipeId = 10;
            dto.IngredientId = 5;
            dto.Quantity = 2.5;
            dto.Unit = "cups";
            dto.CreatedAt = createdAt;

            // Assert
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual(10, dto.RecipeId);
            Assert.AreEqual(5, dto.IngredientId);
            Assert.AreEqual(2.5, dto.Quantity);
            Assert.AreEqual("cups", dto.Unit);
            Assert.AreEqual(createdAt, dto.CreatedAt);
        }

        [TestMethod]
        public void RecipeIngredientDTO_InitializeWithObjectInitializer()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);

            // Act
            var dto = new RecipeIngredientDTO
            {
                Id = 3,
                RecipeId = 15,
                IngredientId = 8,
                Quantity = 1.0,
                Unit = "tablespoon",
                CreatedAt = createdAt
            };

            // Assert
            Assert.AreEqual(3, dto.Id);
            Assert.AreEqual(15, dto.RecipeId);
            Assert.AreEqual(8, dto.IngredientId);
            Assert.AreEqual(1.0, dto.Quantity);
            Assert.AreEqual("tablespoon", dto.Unit);
            Assert.AreEqual(createdAt, dto.CreatedAt);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithZeroQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 0.0 };

            // Assert
            Assert.AreEqual(0.0, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithSmallQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 0.5 };

            // Assert
            Assert.AreEqual(0.5, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithNullUnit()
        {
            // Act
            var dto = new RecipeIngredientDTO { Unit = null };

            // Assert
            Assert.IsNull(dto.Unit);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithEmptyUnit()
        {
            // Act
            var dto = new RecipeIngredientDTO { Unit = string.Empty };

            // Assert
            Assert.AreEqual(string.Empty, dto.Unit);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithMediumQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 50.5 };

            // Assert
            Assert.AreEqual(50.5, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithLargerQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 100.75 };

            // Assert
            Assert.AreEqual(100.75, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithMultipleQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 250.25 };

            // Assert
            Assert.AreEqual(250.25, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithVariousUnits()
        {
            // Act & Assert
            var grams = new RecipeIngredientDTO { Unit = "grams" };
            Assert.AreEqual("grams", grams.Unit);

            var cups = new RecipeIngredientDTO { Unit = "cups" };
            Assert.AreEqual("cups", cups.Unit);

            var teaspoon = new RecipeIngredientDTO { Unit = "teaspoon" };
            Assert.AreEqual("teaspoon", teaspoon.Unit);

            var tablespoon = new RecipeIngredientDTO { Unit = "tablespoon" };
            Assert.AreEqual("tablespoon", tablespoon.Unit);

            var ml = new RecipeIngredientDTO { Unit = "ml" };
            Assert.AreEqual("ml", ml.Unit);

            var kg = new RecipeIngredientDTO { Unit = "kg" };
            Assert.AreEqual("kg", kg.Unit);

            var oz = new RecipeIngredientDTO { Unit = "oz" };
            Assert.AreEqual("oz", oz.Unit);

            var lbs = new RecipeIngredientDTO { Unit = "lbs" };
            Assert.AreEqual("lbs", lbs.Unit);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithZeroValue()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 0.0 };

            // Assert
            Assert.AreEqual(0.0, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithNegativeQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = -5.5 };

            // Assert
            Assert.AreEqual(-5.5, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithLargeQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 1000.0 };

            // Assert
            Assert.AreEqual(1000.0, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithSmallRangeQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 2.5 };

            // Assert
            Assert.AreEqual(2.5, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_WithTypicalQuantity()
        {
            // Act
            var dto = new RecipeIngredientDTO { Quantity = 5.0 };

            // Assert
            Assert.AreEqual(5.0, dto.Quantity);
        }

        [TestMethod]
        public void RecipeIngredientDTO_CreatedAtDefaultValue()
        {
            // Act
            var dto = new RecipeIngredientDTO();

            // Assert
            Assert.AreEqual(DateTimeOffset.MinValue, dto.CreatedAt);
        }

        [TestMethod]
        public void RecipeIngredientDTO_MultipleIngredientsForSameRecipe()
        {
            // Act
            var ingredient1 = new RecipeIngredientDTO { RecipeId = 1, IngredientId = 10, Quantity = 2.5, Unit = "cups" };
            var ingredient2 = new RecipeIngredientDTO { RecipeId = 1, IngredientId = 11, Quantity = 1.0, Unit = "teaspoon" };
            var ingredient3 = new RecipeIngredientDTO { RecipeId = 1, IngredientId = 12, Quantity = 500.0, Unit = "grams" };

            // Assert
            Assert.AreEqual(1, ingredient1.RecipeId);
            Assert.AreEqual(1, ingredient2.RecipeId);
            Assert.AreEqual(1, ingredient3.RecipeId);
            Assert.AreEqual(10, ingredient1.IngredientId);
            Assert.AreEqual(11, ingredient2.IngredientId);
            Assert.AreEqual(12, ingredient3.IngredientId);
        }
    }
}
