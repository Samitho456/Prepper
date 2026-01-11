using Prepper.DTOs;

namespace UnitTests.DTOTests
{
    [TestClass]
    public class MealPlanDTOTests
    {
        [TestMethod]
        public void CreateMealPlanDTO_WithDefaultConstructor()
        {
            // Act
            var dto = new MealPlanDTO();

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Id);
            Assert.AreEqual(default(DateTimeOffset), dto.CreatedAt);
            Assert.IsFalse(dto.IsConsumed);
            Assert.AreEqual(0, dto.UserId);
            Assert.AreEqual(0, dto.RecipeId);
            Assert.IsNull(dto.MealType);
            Assert.AreEqual(default(DateOnly), dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_SetAllProperties_Success()
        {
            // Arrange
            var dto = new MealPlanDTO();
            var createdAt = DateTimeOffset.Now;
            var date = new DateOnly(2024, 6, 15);

            // Act
            dto.Id = 1;
            dto.CreatedAt = createdAt;
            dto.IsConsumed = true;
            dto.UserId = 10;
            dto.RecipeId = 5;
            dto.MealType = "Breakfast";
            dto.Date = date;

            // Assert
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.IsTrue(dto.IsConsumed);
            Assert.AreEqual(10, dto.UserId);
            Assert.AreEqual(5, dto.RecipeId);
            Assert.AreEqual("Breakfast", dto.MealType);
            Assert.AreEqual(date, dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_InitializeWithObjectInitializer()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);
            var date = new DateOnly(2024, 1, 15);

            // Act
            var dto = new MealPlanDTO
            {
                Id = 5,
                CreatedAt = createdAt,
                IsConsumed = false,
                UserId = 25,
                RecipeId = 12,
                MealType = "Lunch",
                Date = date
            };

            // Assert
            Assert.AreEqual(5, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.IsFalse(dto.IsConsumed);
            Assert.AreEqual(25, dto.UserId);
            Assert.AreEqual(12, dto.RecipeId);
            Assert.AreEqual("Lunch", dto.MealType);
            Assert.AreEqual(date, dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_WithEmptyMealType()
        {
            // Act
            var dto = new MealPlanDTO
            {
                MealType = string.Empty
            };

            // Assert
            Assert.AreEqual(string.Empty, dto.MealType);
        }

        [TestMethod]
        public void MealPlanDTO_WithNullMealType()
        {
            // Act
            var dto = new MealPlanDTO
            {
                MealType = null
            };

            // Assert
            Assert.IsNull(dto.MealType);
        }

        [TestMethod]
        public void MealPlanDTO_IsConsumed_DefaultsToFalse()
        {
            // Act
            var dto = new MealPlanDTO();

            // Assert
            Assert.IsFalse(dto.IsConsumed);
        }

        [TestMethod]
        public void MealPlanDTO_IsConsumed_CanBeSetToTrue()
        {
            // Act
            var dto = new MealPlanDTO { IsConsumed = true };

            // Assert
            Assert.IsTrue(dto.IsConsumed);
        }

        [TestMethod]
        public void MealPlanDTO_ToggleIsConsumed()
        {
            // Arrange
            var dto = new MealPlanDTO { IsConsumed = false };

            // Act & Assert
            Assert.IsFalse(dto.IsConsumed);

            dto.IsConsumed = true;
            Assert.IsTrue(dto.IsConsumed);

            dto.IsConsumed = false;
            Assert.IsFalse(dto.IsConsumed);
        }

        [TestMethod]
        public void MealPlanDTO_WithDifferentMealTypes()
        {
            // Act & Assert
            var breakfast = new MealPlanDTO { MealType = "Breakfast" };
            Assert.AreEqual("Breakfast", breakfast.MealType);

            var lunch = new MealPlanDTO { MealType = "Lunch" };
            Assert.AreEqual("Lunch", lunch.MealType);

            var dinner = new MealPlanDTO { MealType = "Dinner" };
            Assert.AreEqual("Dinner", dinner.MealType);

            var snack = new MealPlanDTO { MealType = "Snack" };
            Assert.AreEqual("Snack", snack.MealType);
        }

        [TestMethod]
        public void MealPlanDTO_WithZeroUserId()
        {
            // Act
            var dto = new MealPlanDTO { UserId = 0 };

            // Assert
            Assert.AreEqual(0, dto.UserId);
        }

        [TestMethod]
        public void MealPlanDTO_WithNegativeUserId()
        {
            // Act
            var dto = new MealPlanDTO { UserId = -1 };

            // Assert
            Assert.AreEqual(-1, dto.UserId);
        }

        [TestMethod]
        public void MealPlanDTO_WithZeroRecipeId()
        {
            // Act
            var dto = new MealPlanDTO { RecipeId = 0 };

            // Assert
            Assert.AreEqual(0, dto.RecipeId);
        }

        [TestMethod]
        public void MealPlanDTO_WithNegativeRecipeId()
        {
            // Act
            var dto = new MealPlanDTO { RecipeId = -5 };

            // Assert
            Assert.AreEqual(-5, dto.RecipeId);
        }

        [TestMethod]
        public void MealPlanDTO_WithLargeUserId()
        {
            // Act
            var dto = new MealPlanDTO { UserId = 999999 };

            // Assert
            Assert.AreEqual(999999, dto.UserId);
        }

        [TestMethod]
        public void MealPlanDTO_WithLargeRecipeId()
        {
            // Act
            var dto = new MealPlanDTO { RecipeId = 888888 };

            // Assert
            Assert.AreEqual(888888, dto.RecipeId);
        }

        [TestMethod]
        public void MealPlanDTO_Date_SetAndGet()
        {
            // Arrange
            var dto = new MealPlanDTO();
            var date = new DateOnly(2024, 12, 25);

            // Act
            dto.Date = date;

            // Assert
            Assert.AreEqual(date, dto.Date);
            Assert.AreEqual(2024, dto.Date.Year);
            Assert.AreEqual(12, dto.Date.Month);
            Assert.AreEqual(25, dto.Date.Day);
        }

        [TestMethod]
        public void MealPlanDTO_Date_DefaultValue()
        {
            // Act
            var dto = new MealPlanDTO();

            // Assert
            Assert.AreEqual(default(DateOnly), dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_Date_MinValue()
        {
            // Act
            var dto = new MealPlanDTO { Date = DateOnly.MinValue };

            // Assert
            Assert.AreEqual(DateOnly.MinValue, dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_Date_MaxValue()
        {
            // Act
            var dto = new MealPlanDTO { Date = DateOnly.MaxValue };

            // Assert
            Assert.AreEqual(DateOnly.MaxValue, dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_Date_Today()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);

            // Act
            var dto = new MealPlanDTO { Date = today };

            // Assert
            Assert.AreEqual(today, dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_CreatedAt_DefaultValue()
        {
            // Act
            var dto = new MealPlanDTO();

            // Assert
            Assert.AreEqual(DateTimeOffset.MinValue, dto.CreatedAt);
        }

        [TestMethod]
        public void MealPlanDTO_CreatedAt_SetAndGet()
        {
            // Arrange
            var dto = new MealPlanDTO();
            var createdAt = new DateTimeOffset(2024, 3, 15, 14, 30, 0, TimeSpan.Zero);

            // Act
            dto.CreatedAt = createdAt;

            // Assert
            Assert.AreEqual(createdAt, dto.CreatedAt);
        }

        [TestMethod]
        public void MealPlanDTO_CreatedAt_WithOffset()
        {
            // Arrange
            var offset = TimeSpan.FromHours(-5);
            var createdAt = new DateTimeOffset(2024, 6, 1, 12, 0, 0, offset);

            // Act
            var dto = new MealPlanDTO { CreatedAt = createdAt };

            // Assert
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.AreEqual(offset, dto.CreatedAt.Offset);
        }

        [TestMethod]
        public void MealPlanDTO_CompleteScenario_DinnerPlan()
        {
            // Arrange
            var date = new DateOnly(2024, 6, 20);
            var createdAt = new DateTimeOffset(2024, 6, 15, 10, 0, 0, TimeSpan.Zero);

            // Act
            var dto = new MealPlanDTO
            {
                Id = 10,
                CreatedAt = createdAt,
                IsConsumed = false,
                UserId = 42,
                RecipeId = 15,
                MealType = "Dinner",
                Date = date
            };

            // Assert
            Assert.AreEqual(10, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.IsFalse(dto.IsConsumed);
            Assert.AreEqual(42, dto.UserId);
            Assert.AreEqual(15, dto.RecipeId);
            Assert.AreEqual("Dinner", dto.MealType);
            Assert.AreEqual(date, dto.Date);
        }

        [TestMethod]
        public void MealPlanDTO_CompleteScenario_ConsumedBreakfast()
        {
            // Arrange
            var date = new DateOnly(2024, 1, 1);
            var createdAt = new DateTimeOffset(2023, 12, 31, 20, 0, 0, TimeSpan.Zero);

            // Act
            var dto = new MealPlanDTO
            {
                Id = 100,
                CreatedAt = createdAt,
                IsConsumed = true,
                UserId = 1,
                RecipeId = 50,
                MealType = "Breakfast",
                Date = date
            };

            // Assert
            Assert.AreEqual(100, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.IsTrue(dto.IsConsumed);
            Assert.AreEqual(1, dto.UserId);
            Assert.AreEqual(50, dto.RecipeId);
            Assert.AreEqual("Breakfast", dto.MealType);
            Assert.AreEqual(date, dto.Date);
        }
    }
}
