using Prepper.DTOs;

namespace UnitTests.DTOTests
{
    [TestClass]
    public class RecipeDTOTests
    {
        [TestMethod]
        public void CreateRecipeDTO_WithDefaultConstructor()
        {
            // Act
            var dto = new RecipeDTO();

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Id);
            Assert.IsNull(dto.Title);
            Assert.IsNull(dto.Description);
            Assert.AreEqual(0, dto.Servings);
            Assert.IsNull(dto.MealType);
            Assert.AreEqual(0, dto.PreparationTimeMinutes);
        }

        [TestMethod]
        public void RecipeDTO_SetAllProperties_Success()
        {
            // Arrange
            var dto = new RecipeDTO();
            var createdAt = DateTimeOffset.Now;

            // Act
            dto.Id = 1;
            dto.CreatedAt = createdAt;
            dto.Title = "Pasta Carbonara";
            dto.Description = "Italian classic pasta dish";
            dto.Servings = 4;
            dto.MealType = "Dinner";
            dto.PreparationTimeMinutes = 30;

            // Assert
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.AreEqual("Pasta Carbonara", dto.Title);
            Assert.AreEqual("Italian classic pasta dish", dto.Description);
            Assert.AreEqual(4, dto.Servings);
            Assert.AreEqual("Dinner", dto.MealType);
            Assert.AreEqual(30, dto.PreparationTimeMinutes);
        }

        [TestMethod]
        public void RecipeDTO_InitializeWithObjectInitializer()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);

            // Act
            var dto = new RecipeDTO
            {
                Id = 5,
                CreatedAt = createdAt,
                Title = "Chicken Soup",
                Description = "Warm comfort food",
                Servings = 6,
                MealType = "Lunch",
                PreparationTimeMinutes = 45
            };

            // Assert
            Assert.AreEqual(5, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.AreEqual("Chicken Soup", dto.Title);
            Assert.AreEqual("Warm comfort food", dto.Description);
            Assert.AreEqual(6, dto.Servings);
            Assert.AreEqual("Lunch", dto.MealType);
            Assert.AreEqual(45, dto.PreparationTimeMinutes);
        }

        [TestMethod]
        public void RecipeDTO_WithEmptyStrings()
        {
            // Act
            var dto = new RecipeDTO
            {
                Title = string.Empty,
                Description = string.Empty,
                MealType = string.Empty
            };

            // Assert
            Assert.AreEqual(string.Empty, dto.Title);
            Assert.AreEqual(string.Empty, dto.Description);
            Assert.AreEqual(string.Empty, dto.MealType);
        }

        [TestMethod]
        public void RecipeDTO_WithNullStrings()
        {
            // Act
            var dto = new RecipeDTO
            {
                Title = null,
                Description = null,
                MealType = null
            };

            // Assert
            Assert.IsNull(dto.Title);
            Assert.IsNull(dto.Description);
            Assert.IsNull(dto.MealType);
        }

        [TestMethod]
        public void RecipeDTO_WithZeroServings()
        {
            // Act
            var dto = new RecipeDTO { Servings = 0 };

            // Assert
            Assert.AreEqual(0, dto.Servings);
        }

        [TestMethod]
        public void RecipeDTO_WithNegativeServings()
        {
            // Act
            var dto = new RecipeDTO { Servings = -5 };

            // Assert
            Assert.AreEqual(-5, dto.Servings);
        }

        [TestMethod]
        public void RecipeDTO_WithLargeServings()
        {
            // Act
            var dto = new RecipeDTO { Servings = 100 };

            // Assert
            Assert.AreEqual(100, dto.Servings);
        }

        [TestMethod]
        public void RecipeDTO_WithZeroPreparationTime()
        {
            // Act
            var dto = new RecipeDTO { PreparationTimeMinutes = 0 };

            // Assert
            Assert.AreEqual(0, dto.PreparationTimeMinutes);
        }

        [TestMethod]
        public void RecipeDTO_WithNegativePreparationTime()
        {
            // Act
            var dto = new RecipeDTO { PreparationTimeMinutes = -10 };

            // Assert
            Assert.AreEqual(-10, dto.PreparationTimeMinutes);
        }

        [TestMethod]
        public void RecipeDTO_WithLongPreparationTime()
        {
            // Act
            var dto = new RecipeDTO { PreparationTimeMinutes = 360 };

            // Assert
            Assert.AreEqual(360, dto.PreparationTimeMinutes);
        }

        [TestMethod]
        public void RecipeDTO_WithDifferentMealTypes()
        {
            // Act & Assert
            var breakfast = new RecipeDTO { MealType = "Breakfast" };
            Assert.AreEqual("Breakfast", breakfast.MealType);

            var lunch = new RecipeDTO { MealType = "Lunch" };
            Assert.AreEqual("Lunch", lunch.MealType);

            var dinner = new RecipeDTO { MealType = "Dinner" };
            Assert.AreEqual("Dinner", dinner.MealType);

            var snack = new RecipeDTO { MealType = "Snack" };
            Assert.AreEqual("Snack", snack.MealType);

            var dessert = new RecipeDTO { MealType = "Dessert" };
            Assert.AreEqual("Dessert", dessert.MealType);
        }

        [TestMethod]
        public void RecipeDTO_WithLongTitle()
        {
            // Arrange
            var longTitle = new string('A', 500);

            // Act
            var dto = new RecipeDTO { Title = longTitle };

            // Assert
            Assert.AreEqual(longTitle, dto.Title);
            Assert.AreEqual(500, dto.Title.Length);
        }

        [TestMethod]
        public void RecipeDTO_WithLongDescription()
        {
            // Arrange
            var longDescription = new string('B', 1000);

            // Act
            var dto = new RecipeDTO { Description = longDescription };

            // Assert
            Assert.AreEqual(longDescription, dto.Description);
            Assert.AreEqual(1000, dto.Description.Length);
        }

        [TestMethod]
        public void RecipeDTO_CreatedAtDefaultValue()
        {
            // Act
            var dto = new RecipeDTO();

            // Assert
            Assert.AreEqual(DateTimeOffset.MinValue, dto.CreatedAt);
        }
    }
}
