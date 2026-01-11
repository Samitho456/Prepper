using Prepper.Models;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class MealPlanModelTests
    {
        [TestMethod]
        public void CreateValidMealPlan_WithParameterizedConstructor()
        {
            // Arrange
            var id = 1;
            var createdAt = DateTimeOffset.Now;
            var isConsumed = false;
            var userId = 10;
            var recipeId = 5;
            var mealType = "Breakfast";

            // Act
            var mealPlan = new MealPlan(id, createdAt, isConsumed, userId, recipeId, mealType);

            // Assert
            Assert.AreEqual(id, mealPlan.Id);
            Assert.AreEqual(createdAt, mealPlan.CreatedAt);
            Assert.AreEqual(isConsumed, mealPlan.IsConsumed);
            Assert.AreEqual(userId, mealPlan.UserId);
            Assert.AreEqual(recipeId, mealPlan.RecipeId);
            Assert.AreEqual(mealType, mealPlan.MealType);
        }

        [TestMethod]
        public void CreateValidMealPlan_WithDefaultConstructor()
        {
            // Act
            var mealPlan = new MealPlan();

            // Assert
            Assert.IsNotNull(mealPlan);
            Assert.AreEqual(0, mealPlan.Id);
            Assert.AreEqual(default(DateTimeOffset), mealPlan.CreatedAt);
            Assert.IsFalse(mealPlan.IsConsumed);
            Assert.AreEqual(0, mealPlan.UserId);
            Assert.AreEqual(0, mealPlan.RecipeId);
            Assert.IsNull(mealPlan.MealType);
        }

        [TestMethod]
        public void MealPlan_SetProperties_Success()
        {
            // Arrange
            var mealPlan = new MealPlan();
            var date = new DateOnly(2024, 6, 15);

            // Act
            mealPlan.Id = 3;
            mealPlan.CreatedAt = DateTimeOffset.Now;
            mealPlan.IsConsumed = true;
            mealPlan.UserId = 25;
            mealPlan.RecipeId = 12;
            mealPlan.MealType = "Lunch";
            mealPlan.Date = date;

            // Assert
            Assert.AreEqual(3, mealPlan.Id);
            Assert.IsTrue(mealPlan.IsConsumed);
            Assert.AreEqual(25, mealPlan.UserId);
            Assert.AreEqual(12, mealPlan.RecipeId);
            Assert.AreEqual("Lunch", mealPlan.MealType);
            Assert.AreEqual(date, mealPlan.Date);
        }

        [TestMethod]
        public void MealPlan_ToString_ReturnsFormattedString()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
            var mealPlan = new MealPlan(1, createdAt, false, 10, 5, "Dinner");

            // Act
            var result = mealPlan.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Id: 1"));
            Assert.IsTrue(result.Contains("IsConsumed: False"));
            Assert.IsTrue(result.Contains("UserId: 10"));
            Assert.IsTrue(result.Contains("RecipeId: 5"));
            Assert.IsTrue(result.Contains("MealType: Dinner"));
        }

        [TestMethod]
        public void MealPlan_IsConsumed_DefaultsToFalse()
        {
            // Act
            var mealPlan = new MealPlan();

            // Assert
            Assert.IsFalse(mealPlan.IsConsumed);
        }

        [TestMethod]
        public void MealPlan_IsConsumed_CanBeSetToTrue()
        {
            // Arrange
            var mealPlan = new MealPlan();

            // Act
            mealPlan.IsConsumed = true;

            // Assert
            Assert.IsTrue(mealPlan.IsConsumed);
        }

        [TestMethod]
        public void MealPlan_WithDifferentMealTypes()
        {
            // Act & Assert
            var breakfast = new MealPlan { MealType = "Breakfast" };
            Assert.AreEqual("Breakfast", breakfast.MealType);

            var lunch = new MealPlan { MealType = "Lunch" };
            Assert.AreEqual("Lunch", lunch.MealType);

            var dinner = new MealPlan { MealType = "Dinner" };
            Assert.AreEqual("Dinner", dinner.MealType);

            var snack = new MealPlan { MealType = "Snack" };
            Assert.AreEqual("Snack", snack.MealType);
        }

        [TestMethod]
        public void MealPlan_WithZeroUserIdAndRecipeId()
        {
            // Act
            var mealPlan = new MealPlan(1, DateTimeOffset.Now, false, 0, 0, "Breakfast");

            // Assert
            Assert.AreEqual(0, mealPlan.UserId);
            Assert.AreEqual(0, mealPlan.RecipeId);
        }

        [TestMethod]
        public void MealPlan_WithNegativeUserIdAndRecipeId()
        {
            // Act
            var mealPlan = new MealPlan();
            mealPlan.UserId = -1;
            mealPlan.RecipeId = -5;

            // Assert
            Assert.AreEqual(-1, mealPlan.UserId);
            Assert.AreEqual(-5, mealPlan.RecipeId);
        }

        [TestMethod]
        public void MealPlan_WithNullMealType()
        {
            // Act
            var mealPlan = new MealPlan { MealType = null };

            // Assert
            Assert.IsNull(mealPlan.MealType);
        }

        [TestMethod]
        public void MealPlan_WithEmptyMealType()
        {
            // Act
            var mealPlan = new MealPlan { MealType = string.Empty };

            // Assert
            Assert.AreEqual(string.Empty, mealPlan.MealType);
        }

        [TestMethod]
        public void MealPlan_Date_SetAndGet()
        {
            // Arrange
            var mealPlan = new MealPlan();
            var date = new DateOnly(2024, 12, 25);

            // Act
            mealPlan.Date = date;

            // Assert
            Assert.AreEqual(date, mealPlan.Date);
            Assert.AreEqual(2024, mealPlan.Date.Year);
            Assert.AreEqual(12, mealPlan.Date.Month);
            Assert.AreEqual(25, mealPlan.Date.Day);
        }

        [TestMethod]
        public void MealPlan_Date_DefaultValue()
        {
            // Act
            var mealPlan = new MealPlan();

            // Assert
            Assert.AreEqual(default(DateOnly), mealPlan.Date);
        }

        [TestMethod]
        public void MealPlan_Date_MinValue()
        {
            // Arrange
            var mealPlan = new MealPlan();

            // Act
            mealPlan.Date = DateOnly.MinValue;

            // Assert
            Assert.AreEqual(DateOnly.MinValue, mealPlan.Date);
        }

        [TestMethod]
        public void MealPlan_Date_MaxValue()
        {
            // Arrange
            var mealPlan = new MealPlan();

            // Act
            mealPlan.Date = DateOnly.MaxValue;

            // Assert
            Assert.AreEqual(DateOnly.MaxValue, mealPlan.Date);
        }

        [TestMethod]
        public void MealPlan_CreatedAt_CanBeSet()
        {
            // Arrange
            var mealPlan = new MealPlan();
            var createdAt = new DateTimeOffset(2024, 3, 15, 14, 30, 0, TimeSpan.Zero);

            // Act
            mealPlan.CreatedAt = createdAt;

            // Assert
            Assert.AreEqual(createdAt, mealPlan.CreatedAt);
        }

        [TestMethod]
        public void MealPlan_ToggleIsConsumed()
        {
            // Arrange
            var mealPlan = new MealPlan { IsConsumed = false };

            // Act & Assert
            Assert.IsFalse(mealPlan.IsConsumed);

            mealPlan.IsConsumed = true;
            Assert.IsTrue(mealPlan.IsConsumed);

            mealPlan.IsConsumed = false;
            Assert.IsFalse(mealPlan.IsConsumed);
        }
    }
}
