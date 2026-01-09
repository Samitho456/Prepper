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
            var quantity = "2.5";
            var unit = "cups";
            var createdAt = DateTimeOffset.Now;

            // Act
            var recipeIngredient = new RecipeIngredients(id, recipeId, ingredientId, quantity, unit, createdAt);

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
            var recipeIngredient = new RecipeIngredients();

            // Assert
            Assert.IsNotNull(recipeIngredient);
            Assert.AreEqual(0, recipeIngredient.Id);
            Assert.AreEqual(0, recipeIngredient.RecipeId);
            Assert.AreEqual(0, recipeIngredient.IngredientId);
            Assert.IsNull(recipeIngredient.Quantity);
            Assert.IsNull(recipeIngredient.Unit);
        }

        [TestMethod]
        public void RecipeIngredients_SetProperties_Success()
        {
            // Arrange
            var recipeIngredient = new RecipeIngredients();
            var createdAt = DateTimeOffset.Now;

            // Act
            recipeIngredient.Id = 3;
            recipeIngredient.RecipeId = 15;
            recipeIngredient.IngredientId = 8;
            recipeIngredient.Quantity = "1";
            recipeIngredient.Unit = "tablespoon";
            recipeIngredient.CreatedAt = createdAt;

            // Assert
            Assert.AreEqual(3, recipeIngredient.Id);
            Assert.AreEqual(15, recipeIngredient.RecipeId);
            Assert.AreEqual(8, recipeIngredient.IngredientId);
            Assert.AreEqual("1", recipeIngredient.Quantity);
            Assert.AreEqual("tablespoon", recipeIngredient.Unit);
            Assert.AreEqual(createdAt, recipeIngredient.CreatedAt);
        }

        [TestMethod]
        public void RecipeIngredients_ToString_ReturnsFormattedString()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
            var recipeIngredient = new RecipeIngredients(1, 5, 10, "3", "grams", createdAt);

            // Act
            var result = recipeIngredient.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Id: 1"));
            Assert.IsTrue(result.Contains("RecipeId: 5"));
            Assert.IsTrue(result.Contains("IngredientId: 10"));
            Assert.IsTrue(result.Contains("Quantity: 3 grams"));
        }

        [TestMethod]
        public void RecipeIngredients_WithNullQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredients(1, 1, 1, null, "cups", DateTimeOffset.Now);

            // Assert
            Assert.IsNull(recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithNullUnit()
        {
            // Act
            var recipeIngredient = new RecipeIngredients(1, 1, 1, "100", null, DateTimeOffset.Now);

            // Assert
            Assert.IsNull(recipeIngredient.Unit);
        }

        [TestMethod]
        public void RecipeIngredients_WithEmptyQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredients(1, 1, 1, string.Empty, "ml", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(string.Empty, recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithEmptyUnit()
        {
            // Act
            var recipeIngredient = new RecipeIngredients(1, 1, 1, "50", string.Empty, DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(string.Empty, recipeIngredient.Unit);
        }

        [TestMethod]
        public void RecipeIngredients_WithDecimalQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredients(1, 1, 1, "0.5", "kg", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual("0.5", recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithFractionalQuantity()
        {
            // Act
            var recipeIngredient = new RecipeIngredients(1, 1, 1, "1/2", "cup", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual("1/2", recipeIngredient.Quantity);
        }

        [TestMethod]
        public void RecipeIngredients_WithVariousUnits()
        {
            // Arrange & Act
            var ingredient1 = new RecipeIngredients(1, 1, 1, "100", "grams", DateTimeOffset.Now);
            var ingredient2 = new RecipeIngredients(2, 1, 2, "2", "cups", DateTimeOffset.Now);
            var ingredient3 = new RecipeIngredients(3, 1, 3, "1", "teaspoon", DateTimeOffset.Now);
            var ingredient4 = new RecipeIngredients(4, 1, 4, "250", "ml", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual("grams", ingredient1.Unit);
            Assert.AreEqual("cups", ingredient2.Unit);
            Assert.AreEqual("teaspoon", ingredient3.Unit);
            Assert.AreEqual("ml", ingredient4.Unit);
        }
    }
}
