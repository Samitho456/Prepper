using Prepper.DTOs;

namespace UnitTests.DTOTests
{
    [TestClass]
    public class NutritionalProfileDTOTests
    {
        [TestMethod]
        public void CreateNutritionalProfileDTO_WithDefaultConstructor()
        {
            // Act
            var dto = new NutritionalProfileDTO();

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(0, dto.Id);
            Assert.AreEqual(0, dto.IngredientId);
            Assert.AreEqual(0, dto.UnitAmount);
            Assert.IsNull(dto.BaseUnit);
            Assert.IsNull(dto.Kcal);
            Assert.IsNull(dto.Kj);
            Assert.IsNull(dto.FatTotal);
            Assert.IsNull(dto.FatSaturated);
            Assert.IsNull(dto.CarbohydrateTotal);
            Assert.IsNull(dto.CarbohydrateSugars);
            Assert.IsNull(dto.Fiber);
            Assert.IsNull(dto.Protein);
            Assert.IsNull(dto.Salt);
        }

        [TestMethod]
        public void NutritionalProfileDTO_SetAllProperties_Success()
        {
            // Arrange
            var dto = new NutritionalProfileDTO();
            var createdAt = DateTimeOffset.Now;

            // Act
            dto.Id = 1;
            dto.CreatedAt = createdAt;
            dto.IngredientId = 5;
            dto.UnitAmount = 100;
            dto.BaseUnit = "grams";
            dto.Kcal = 250.5f;
            dto.Kj = 1047.0f;
            dto.FatTotal = 10.2f;
            dto.FatSaturated = 3.5f;
            dto.CarbohydrateTotal = 30.0f;
            dto.CarbohydrateSugars = 5.5f;
            dto.Fiber = 4.0f;
            dto.Protein = 8.0f;
            dto.Salt = 1.2f;

            // Assert
            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.AreEqual(5, dto.IngredientId);
            Assert.AreEqual(100, dto.UnitAmount);
            Assert.AreEqual("grams", dto.BaseUnit);
            Assert.AreEqual(250.5f, dto.Kcal);
            Assert.AreEqual(1047.0f, dto.Kj);
            Assert.AreEqual(10.2f, dto.FatTotal);
            Assert.AreEqual(3.5f, dto.FatSaturated);
            Assert.AreEqual(30.0f, dto.CarbohydrateTotal);
            Assert.AreEqual(5.5f, dto.CarbohydrateSugars);
            Assert.AreEqual(4.0f, dto.Fiber);
            Assert.AreEqual(8.0f, dto.Protein);
            Assert.AreEqual(1.2f, dto.Salt);
        }

        [TestMethod]
        public void NutritionalProfileDTO_InitializeWithObjectInitializer()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);

            // Act
            var dto = new NutritionalProfileDTO
            {
                Id = 10,
                CreatedAt = createdAt,
                IngredientId = 15,
                UnitAmount = 50,
                BaseUnit = "ml",
                Kcal = 100.0f,
                Kj = 418.0f,
                FatTotal = 5.0f,
                FatSaturated = 1.0f,
                CarbohydrateTotal = 12.0f,
                CarbohydrateSugars = 2.0f,
                Fiber = 1.5f,
                Protein = 3.0f,
                Salt = 0.5f
            };

            // Assert
            Assert.AreEqual(10, dto.Id);
            Assert.AreEqual(createdAt, dto.CreatedAt);
            Assert.AreEqual(15, dto.IngredientId);
            Assert.AreEqual(50, dto.UnitAmount);
            Assert.AreEqual("ml", dto.BaseUnit);
            Assert.AreEqual(100.0f, dto.Kcal);
            Assert.AreEqual(418.0f, dto.Kj);
            Assert.AreEqual(5.0f, dto.FatTotal);
            Assert.AreEqual(1.0f, dto.FatSaturated);
            Assert.AreEqual(12.0f, dto.CarbohydrateTotal);
            Assert.AreEqual(2.0f, dto.CarbohydrateSugars);
            Assert.AreEqual(1.5f, dto.Fiber);
            Assert.AreEqual(3.0f, dto.Protein);
            Assert.AreEqual(0.5f, dto.Salt);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithNullNutrients()
        {
            // Act
            var dto = new NutritionalProfileDTO
            {
                Kcal = null,
                Kj = null,
                FatTotal = null,
                FatSaturated = null,
                CarbohydrateTotal = null,
                CarbohydrateSugars = null,
                Fiber = null,
                Protein = null,
                Salt = null
            };

            // Assert
            Assert.IsNull(dto.Kcal);
            Assert.IsNull(dto.Kj);
            Assert.IsNull(dto.FatTotal);
            Assert.IsNull(dto.FatSaturated);
            Assert.IsNull(dto.CarbohydrateTotal);
            Assert.IsNull(dto.CarbohydrateSugars);
            Assert.IsNull(dto.Fiber);
            Assert.IsNull(dto.Protein);
            Assert.IsNull(dto.Salt);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithZeroNutrients()
        {
            // Act
            var dto = new NutritionalProfileDTO
            {
                Kcal = 0,
                Kj = 0,
                FatTotal = 0,
                FatSaturated = 0,
                CarbohydrateTotal = 0,
                CarbohydrateSugars = 0,
                Fiber = 0,
                Protein = 0,
                Salt = 0
            };

            // Assert
            Assert.AreEqual(0, dto.Kcal);
            Assert.AreEqual(0, dto.Kj);
            Assert.AreEqual(0, dto.FatTotal);
            Assert.AreEqual(0, dto.FatSaturated);
            Assert.AreEqual(0, dto.CarbohydrateTotal);
            Assert.AreEqual(0, dto.CarbohydrateSugars);
            Assert.AreEqual(0, dto.Fiber);
            Assert.AreEqual(0, dto.Protein);
            Assert.AreEqual(0, dto.Salt);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithHighNutrientValues()
        {
            // Act
            var dto = new NutritionalProfileDTO
            {
                Kcal = 900.0f,
                FatTotal = 100.0f,
                CarbohydrateTotal = 250.0f,
                Protein = 50.0f
            };

            // Assert
            Assert.AreEqual(900.0f, dto.Kcal);
            Assert.AreEqual(100.0f, dto.FatTotal);
            Assert.AreEqual(250.0f, dto.CarbohydrateTotal);
            Assert.AreEqual(50.0f, dto.Protein);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithDecimalValues()
        {
            // Act
            var dto = new NutritionalProfileDTO
            {
                Kcal = 123.45f,
                FatSaturated = 2.67f,
                CarbohydrateSugars = 8.92f,
                Fiber = 3.14f
            };

            // Assert
            Assert.AreEqual(123.45f, dto.Kcal);
            Assert.AreEqual(2.67f, dto.FatSaturated);
            Assert.AreEqual(8.92f, dto.CarbohydrateSugars);
            Assert.AreEqual(3.14f, dto.Fiber);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithNullBaseUnit()
        {
            // Act
            var dto = new NutritionalProfileDTO { BaseUnit = null };

            // Assert
            Assert.IsNull(dto.BaseUnit);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithEmptyBaseUnit()
        {
            // Act
            var dto = new NutritionalProfileDTO { BaseUnit = string.Empty };

            // Assert
            Assert.AreEqual(string.Empty, dto.BaseUnit);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithDifferentBaseUnits()
        {
            // Act & Assert
            var grams = new NutritionalProfileDTO { BaseUnit = "grams", UnitAmount = 100 };
            Assert.AreEqual("grams", grams.BaseUnit);
            Assert.AreEqual(100, grams.UnitAmount);

            var ml = new NutritionalProfileDTO { BaseUnit = "ml", UnitAmount = 250 };
            Assert.AreEqual("ml", ml.BaseUnit);
            Assert.AreEqual(250, ml.UnitAmount);

            var oz = new NutritionalProfileDTO { BaseUnit = "oz", UnitAmount = 1 };
            Assert.AreEqual("oz", oz.BaseUnit);
            Assert.AreEqual(1, oz.UnitAmount);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithZeroUnitAmount()
        {
            // Act
            var dto = new NutritionalProfileDTO { UnitAmount = 0 };

            // Assert
            Assert.AreEqual(0, dto.UnitAmount);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithNegativeUnitAmount()
        {
            // Act
            var dto = new NutritionalProfileDTO { UnitAmount = -100 };

            // Assert
            Assert.AreEqual(-100, dto.UnitAmount);
        }

        [TestMethod]
        public void NutritionalProfileDTO_WithLargeUnitAmount()
        {
            // Act
            var dto = new NutritionalProfileDTO { UnitAmount = 1000 };

            // Assert
            Assert.AreEqual(1000, dto.UnitAmount);
        }

        [TestMethod]
        public void NutritionalProfileDTO_KcalToKjConversion()
        {
            // Arrange - 1 kcal ? 4.184 kJ
            var dto = new NutritionalProfileDTO();
            var kcalValue = 100.0f;

            // Act
            dto.Kcal = kcalValue;
            dto.Kj = kcalValue * 4.184f;

            // Assert
            Assert.AreEqual(100.0f, dto.Kcal);
            Assert.AreEqual(418.4f, dto.Kj);
        }

        [TestMethod]
        public void NutritionalProfileDTO_CreatedAtDefaultValue()
        {
            // Act
            var dto = new NutritionalProfileDTO();

            // Assert
            Assert.AreEqual(DateTimeOffset.MinValue, dto.CreatedAt);
        }

        [TestMethod]
        public void NutritionalProfileDTO_PartialNutrientData()
        {
            // Act - Scenario where only some nutrients are available
            var dto = new NutritionalProfileDTO
            {
                Kcal = 150.0f,
                Protein = 5.0f,
                CarbohydrateTotal = 20.0f,
                FatTotal = null,
                FatSaturated = null,
                Fiber = null
            };

            // Assert
            Assert.AreEqual(150.0f, dto.Kcal);
            Assert.AreEqual(5.0f, dto.Protein);
            Assert.AreEqual(20.0f, dto.CarbohydrateTotal);
            Assert.IsNull(dto.FatTotal);
            Assert.IsNull(dto.FatSaturated);
            Assert.IsNull(dto.Fiber);
        }

        [TestMethod]
        public void NutritionalProfileDTO_NegativeNutrientValues()
        {
            // Act
            var dto = new NutritionalProfileDTO
            {
                Kcal = -10.0f,
                Protein = -5.0f
            };

            // Assert
            Assert.AreEqual(-10.0f, dto.Kcal);
            Assert.AreEqual(-5.0f, dto.Protein);
        }

        [TestMethod]
        public void NutritionalProfileDTO_VerySmallNutrientValues()
        {
            // Act
            var dto = new NutritionalProfileDTO
            {
                Salt = 0.001f,
                Fiber = 0.1f,
                FatSaturated = 0.05f
            };

            // Assert
            Assert.AreEqual(0.001f, dto.Salt);
            Assert.AreEqual(0.1f, dto.Fiber);
            Assert.AreEqual(0.05f, dto.FatSaturated);
        }

        [TestMethod]
        public void NutritionalProfileDTO_MacronutrientBalance()
        {
            // Act - Example of a balanced meal's nutritional profile
            var dto = new NutritionalProfileDTO
            {
                UnitAmount = 100,
                BaseUnit = "grams",
                Kcal = 200.0f,
                Protein = 20.0f,      // 80 kcal from protein (4 kcal/g)
                CarbohydrateTotal = 20.0f,  // 80 kcal from carbs (4 kcal/g)
                FatTotal = 4.44f      // ~40 kcal from fat (9 kcal/g)
            };

            // Assert
            Assert.AreEqual(200.0f, dto.Kcal);
            Assert.AreEqual(20.0f, dto.Protein);
            Assert.AreEqual(20.0f, dto.CarbohydrateTotal);
            Assert.AreEqual(4.44f, dto.FatTotal);
        }
    }
}
