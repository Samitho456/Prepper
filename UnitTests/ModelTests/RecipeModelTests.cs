using Prepper.Models;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class RecipeModelTests
    {
        [TestMethod]
        public void CreateValidRecipe_WithParameterizedConstructor()
        {
            // Arrange
            var id = 1;
            var title = "Spaghetti Carbonara";
            var description = "Classic Italian pasta dish";
            var servings = 4;
            var mealType = "Dinner";
            var createdAt = DateTimeOffset.Now;

            // Act
            var recipe = new Recipe(id, title, description, servings, mealType, createdAt);

            // Assert
            Assert.AreEqual(id, recipe.Id);
            Assert.AreEqual(title, recipe.Title);
            Assert.AreEqual(description, recipe.Description);
            Assert.AreEqual(servings, recipe.Servings);
            Assert.AreEqual(mealType, recipe.MealType);
            Assert.AreEqual(createdAt, recipe.CreatedAt);
        }

        [TestMethod]
        public void CreateValidRecipe_WithDefaultConstructor()
        {
            // Act
            var recipe = new Recipe();

            // Assert
            Assert.IsNotNull(recipe);
            Assert.AreEqual(0, recipe.Id);
            Assert.IsNull(recipe.Title);
            Assert.IsNull(recipe.Description);
            Assert.AreEqual(0, recipe.Servings);
            Assert.IsNull(recipe.MealType);
        }

        [TestMethod]
        public void Recipe_SetProperties_Success()
        {
            // Arrange
            var recipe = new Recipe();

            // Act
            recipe.Id = 5;
            recipe.Title = "Pancakes";
            recipe.Description = "Fluffy breakfast pancakes";
            recipe.Servings = 2;
            recipe.PreparationTimeMinutes = 15;
            recipe.MealType = "Breakfast";

            // Assert
            Assert.AreEqual(5, recipe.Id);
            Assert.AreEqual("Pancakes", recipe.Title);
            Assert.AreEqual("Fluffy breakfast pancakes", recipe.Description);
            Assert.AreEqual(2, recipe.Servings);
            Assert.AreEqual(15, recipe.PreparationTimeMinutes);
            Assert.AreEqual("Breakfast", recipe.MealType);
        }

        [TestMethod]
        public void Recipe_ToString_ReturnsFormattedString()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
            var recipe = new Recipe(1, "Test Recipe", "Test Description", 4, "Lunch", createdAt);

            // Act
            var result = recipe.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Id: 1"));
            Assert.IsTrue(result.Contains("Title: Test Recipe"));
            Assert.IsTrue(result.Contains("Servings: 4"));
            Assert.IsTrue(result.Contains("Meal type: Lunch"));
            Assert.IsTrue(result.Contains("Description: Test Description"));
        }

        [TestMethod]
        public void Recipe_WithZeroServings()
        {
            // Act
            var recipe = new Recipe(1, "Test", "Description", 0, "Snack", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(0, recipe.Servings);
        }

        [TestMethod]
        public void Recipe_WithNegativeServings()
        {
            // Act
            var recipe = new Recipe(1, "Test", "Description", -1, "Snack", DateTimeOffset.Now);

            // Assert
            Assert.AreEqual(-1, recipe.Servings);
        }

        [TestMethod]
        public void Recipe_WithNegativePreparationTime()
        {
            // Arrange
            var recipe = new Recipe();

            // Act
            recipe.PreparationTimeMinutes = -10;

            // Assert
            Assert.AreEqual(-10, recipe.PreparationTimeMinutes);
        }
    }
}
