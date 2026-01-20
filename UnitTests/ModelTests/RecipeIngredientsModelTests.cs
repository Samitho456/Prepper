using Prepper.Models;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class RecipeIngredientsModelTests
    {
        [TestMethod]
        public void CreateValidRecipeIngredients_WithParameterizedConstructor()
        {
            // Arrange
            var id = 1;
            var recipeId = 10;
            var ingredientId = 5;
            var quantity = 2.5;
            var unit = "cups";
            var createdAt = DateTimeOffset.Now;

            // Act
            var recipeIngredient = new RecipeIngredient(id, recipeId, ingredientId, quantity, unit, createdAt);

            // Assert
            Assert.AreEqual(id, recipeIngredient.Id);
            Assert.AreEqual(recipeId, recipeIngredient.RecipeId);
            Assert.AreEqual(ingredientId, recipeIngredient.IngredientId);
            Assert.AreEqual(quantity, recipeIngredient.Quantity);
            Assert.AreEqual(unit, recipeIngredient.Unit);
            Assert.AreEqual(createdAt, recipeIngredient.CreatedAt);
        }

        [TestMethod]
        public void CreateValidRecipeIngredients_WithDefaultConstructor()
        {
            // Act
            var recipeIngredient = new RecipeIngredient();

            // Assert
            Assert.IsNotNull(recipeIngredient);
            Assert.AreEqual(0, recipeIngredient.Id);
            Assert.AreEqual(0, recipeIngredient.RecipeId);
            Assert.AreEqual(0, recipeIngredient.IngredientId);
            Assert.AreEqual(0.0, recipeIngredient.Quantity);
            Assert.IsNull(recipeIngredient.Unit);
        }

        [TestMethod]
        public void RecipeIngredients_SetProperties_Success()
        {
            // Arrange
            var recipeIngredient = new RecipeIngredient();
            var createdAt = DateTimeOffset.Now;

            // Act
            recipeIngredient.Id = 3;
            recipeIngredient.RecipeId = 15;
            recipeIngredient.IngredientId = 8;
            recipeIngredient.Quantity = 1.0;
            recipeIngredient.Unit = "tablespoon";
            recipeIngredient.CreatedAt = createdAt;

            // Assert
            Assert.AreEqual(3, recipeIngredient.Id);
            Assert.AreEqual(15, recipeIngredient.RecipeId);
            Assert.AreEqual(8, recipeIngredient.IngredientId);
            Assert.AreEqual(1.0, recipeIngredient.Quantity);
            Assert.AreEqual("tablespoon", recipeIngredient.Unit);
            Assert.AreEqual(createdAt, recipeIngredient.CreatedAt);
        }

        [TestMethod]
        public void RecipeIngredients_ToString_ReturnsFormattedString()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
            var recipeIngredient = new RecipeIngredient(1, 5, 10, 3.0, "grams", createdAt);

            // Act
            var result = recipeIngredient.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Id: 1"));
            Assert.IsTrue(result.Contains("RecipeId: 5"));
            Assert.IsTrue(result.Contains("IngredientId: 10"));
            Assert.IsTrue(result.Contains("Quantity: 3 grams"));
        }

        [TestMethod]
        public void RecipeIngredients_WithZeroQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredient(1, 1, 1, 0.0, "cups", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(0.0, recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithNullUnit()
        {
            // Act
            var recipeIngredient = new RecipeIngredient(1, 1, 1, 100.0, null, DateTimeOffset.Now);

            // Assert
            Assert.IsNull(recipeIngredient.Unit);
        }

        [TestMethod]
        public void RecipeIngredients_WithSmallQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredient(1, 1, 1, 0.5, "ml", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(0.5, recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithEmptyUnit()
        {
            // Act
            var recipeIngredient = new RecipeIngredient(1, 1, 1, 50.0, string.Empty, DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(string.Empty, recipeIngredient.Unit);
        }

        [TestMethod]
        public void RecipeIngredients_WithLargeQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredient(1, 1, 1, 1000.0, "kg", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(1000.0, recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithMediumQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredient(1, 1, 1, 50.5, "cup", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(50.5, recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithVariousUnits()
        {
            // Arrange & Act
            var ingredient1 = new RecipeIngredient(1, 1, 1, 100.0, "grams", DateTimeOffset.Now);
            var ingredient2 = new RecipeIngredient(2, 1, 2, 2.5, "cups", DateTimeOffset.Now);
            var ingredient3 = new RecipeIngredient(3, 1, 3, 1.0, "teaspoon", DateTimeOffset.Now);
            var ingredient4 = new RecipeIngredient(4, 1, 4, 250.0, "ml", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual("grams", ingredient1.Unit);
            Assert.AreEqual("cups", ingredient2.Unit);
            Assert.AreEqual("teaspoon", ingredient3.Unit);
            Assert.AreEqual("ml", ingredient4.Unit);
        }
    }
}
