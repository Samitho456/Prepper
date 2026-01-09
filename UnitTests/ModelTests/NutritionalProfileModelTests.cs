using Prepper.Models;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class NutritionalProfileModelTests
    {
        [TestMethod]
        public void CreateValidNutritionalProfile_WithParameterizedConstructor()
        {
            // Arrange
            var id = 1;
            var createdAt = DateTimeOffset.Now;
            var ingredientId = 5;
            var unitAmount = 100;
            var baseUnit = "grams";
            var kcal = 250.5f;
            var kj = 1047.0f;
            var fatTotal = 10.2f;
            var fatSaturated = 3.5f;
            var carbTotal = 30.0f;
            var carbSugar = 5.5f;
            var protein = 8.0f;
            var salt = 1.2f;
            var fiber = 4.0f;

            // Act
            var profile = new NutritionalProfile(id, createdAt, ingredientId, unitAmount, baseUnit,
                kcal, kj, fatTotal, fatSaturated, carbTotal, carbSugar, protein, salt, fiber);

            // Assert
            Assert.AreEqual(id, profile.Id);
            Assert.AreEqual(createdAt, profile.CreatedAt);
            Assert.AreEqual(ingredientId, profile.IngredientId);
            Assert.AreEqual(unitAmount, profile.UnitAmount);
            Assert.AreEqual(baseUnit, profile.BaseUnit);
            Assert.AreEqual(kcal, profile.Kcal);
            Assert.AreEqual(kj, profile.Kj);
            Assert.AreEqual(fatTotal, profile.FatTotal);
            Assert.AreEqual(fatSaturated, profile.FatSaturated);
            Assert.AreEqual(carbTotal, profile.CarbohydrateTotal);
            Assert.AreEqual(carbSugar, profile.CarbohydrateSugars);
            Assert.AreEqual(protein, profile.Protein);
            Assert.AreEqual(salt, profile.Salt);
            Assert.AreEqual(fiber, profile.Fiber);
        }

        [TestMethod]
        public void CreateValidNutritionalProfile_WithDefaultConstructor()
        {
            // Act
            var profile = new NutritionalProfile();

            // Assert
            Assert.IsNotNull(profile);
            Assert.AreEqual(0, profile.Id);
            Assert.AreEqual(0, profile.IngredientId);
            Assert.AreEqual(0, profile.UnitAmount);
            Assert.IsNull(profile.BaseUnit);
            Assert.IsNull(profile.Kcal);
            Assert.IsNull(profile.Kj);
            Assert.IsNull(profile.FatTotal);
            Assert.IsNull(profile.FatSaturated);
            Assert.IsNull(profile.CarbohydrateTotal);
            Assert.IsNull(profile.CarbohydrateSugars);
            Assert.IsNull(profile.Protein);
            Assert.IsNull(profile.Salt);
            Assert.IsNull(profile.Fiber);
        }

        [TestMethod]
        public void NutritionalProfile_SetProperties_Success()
        {
            // Arrange
            var profile = new NutritionalProfile();
            var createdAt = DateTimeOffset.Now;

            // Act
            profile.Id = 10;
            profile.CreatedAt = createdAt;
            profile.IngredientId = 15;
            profile.UnitAmount = 50;
            profile.BaseUnit = "ml";
            profile.Kcal = 100.0f;
            profile.Kj = 418.0f;
            profile.FatTotal = 5.0f;
            profile.FatSaturated = 1.0f;
            profile.CarbohydrateTotal = 12.0f;
            profile.CarbohydrateSugars = 2.0f;
            profile.Protein = 3.0f;
            profile.Salt = 0.5f;
            profile.Fiber = 1.5f;

            // Assert
            Assert.AreEqual(10, profile.Id);
            Assert.AreEqual(createdAt, profile.CreatedAt);
            Assert.AreEqual(15, profile.IngredientId);
            Assert.AreEqual(50, profile.UnitAmount);
            Assert.AreEqual("ml", profile.BaseUnit);
            Assert.AreEqual(100.0f, profile.Kcal);
            Assert.AreEqual(418.0f, profile.Kj);
            Assert.AreEqual(5.0f, profile.FatTotal);
            Assert.AreEqual(1.0f, profile.FatSaturated);
            Assert.AreEqual(12.0f, profile.CarbohydrateTotal);
            Assert.AreEqual(2.0f, profile.CarbohydrateSugars);
            Assert.AreEqual(3.0f, profile.Protein);
            Assert.AreEqual(0.5f, profile.Salt);
            Assert.AreEqual(1.5f, profile.Fiber);
        }

        [TestMethod]
        public void NutritionalProfile_ToString_ReturnsFormattedString()
        {
            // Arrange
            var createdAt = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
            var profile = new NutritionalProfile(1, createdAt, 5, 100, "grams",
                250.0f, 1047.0f, 10.0f, 3.0f, 30.0f, 5.0f, 8.0f, 1.0f, 4.0f);

            // Act
            var result = profile.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Id: 1"));
            Assert.IsTrue(result.Contains("IngredientId: 5"));
            Assert.IsTrue(result.Contains("UnitAmount: 100"));
            Assert.IsTrue(result.Contains("BaseUnit: grams"));
            Assert.IsTrue(result.Contains("Kcal: 250"));
            Assert.IsTrue(result.Contains("Protein: 8"));
        }

        [TestMethod]
        public void NutritionalProfile_WithNullableNutrients_AllNull()
        {
            // Act
            var profile = new NutritionalProfile(1, DateTimeOffset.Now, 1, 100, "grams",
                0, 0, 0, 0, 0, 0, 0, 0, 0);

            // Assert
            Assert.AreEqual(0, profile.Kcal);
            Assert.AreEqual(0, profile.Kj);
            Assert.AreEqual(0, profile.FatTotal);
            Assert.AreEqual(0, profile.FatSaturated);
            Assert.AreEqual(0, profile.CarbohydrateTotal);
            Assert.AreEqual(0, profile.CarbohydrateSugars);
            Assert.AreEqual(0, profile.Protein);
            Assert.AreEqual(0, profile.Salt);
            Assert.AreEqual(0, profile.Fiber);
        }

        [TestMethod]
        public void NutritionalProfile_WithZeroValues()
        {
            // Act
            var profile = new NutritionalProfile();
            profile.Kcal = 0;
            profile.FatTotal = 0;
            profile.Protein = 0;

            // Assert
            Assert.AreEqual(0, profile.Kcal);
            Assert.AreEqual(0, profile.FatTotal);
            Assert.AreEqual(0, profile.Protein);
        }

        [TestMethod]
        public void NutritionalProfile_WithHighValues()
        {
            // Act
            var profile = new NutritionalProfile();
            profile.Kcal = 900.0f;
            profile.FatTotal = 100.0f;
            profile.CarbohydrateTotal = 250.0f;
            profile.Protein = 50.0f;

            // Assert
            Assert.AreEqual(900.0f, profile.Kcal);
            Assert.AreEqual(100.0f, profile.FatTotal);
            Assert.AreEqual(250.0f, profile.CarbohydrateTotal);
            Assert.AreEqual(50.0f, profile.Protein);
        }

        [TestMethod]
        public void NutritionalProfile_WithDecimalValues()
        {
            // Act
            var profile = new NutritionalProfile();
            profile.Kcal = 123.45f;
            profile.FatSaturated = 2.67f;
            profile.CarbohydrateSugars = 8.92f;
            profile.Fiber = 3.14f;

            // Assert
            Assert.AreEqual(123.45f, profile.Kcal);
            Assert.AreEqual(2.67f, profile.FatSaturated);
            Assert.AreEqual(8.92f, profile.CarbohydrateSugars);
            Assert.AreEqual(3.14f, profile.Fiber);
        }

        [TestMethod]
        public void NutritionalProfile_WithDifferentBaseUnits()
        {
            // Arrange & Act
            var profile1 = new NutritionalProfile();
            profile1.BaseUnit = "grams";
            profile1.UnitAmount = 100;

            var profile2 = new NutritionalProfile();
            profile2.BaseUnit = "ml";
            profile2.UnitAmount = 250;

            var profile3 = new NutritionalProfile();
            profile3.BaseUnit = "oz";
            profile3.UnitAmount = 1;

            // Assert
            Assert.AreEqual("grams", profile1.BaseUnit);
            Assert.AreEqual(100, profile1.UnitAmount);
            Assert.AreEqual("ml", profile2.BaseUnit);
            Assert.AreEqual(250, profile2.UnitAmount);
            Assert.AreEqual("oz", profile3.BaseUnit);
            Assert.AreEqual(1, profile3.UnitAmount);
        }

        [TestMethod]
        public void NutritionalProfile_KcalToKjConversion_ApproximatelyCorrect()
        {
            // Arrange - 1 kcal ≈ 4.184 kJ
            var profile = new NutritionalProfile();
            var kcalValue = 100.0f;

            // Act
            profile.Kcal = kcalValue;
            profile.Kj = kcalValue * 4.184f;

            // Assert
            Assert.AreEqual(100.0f, profile.Kcal);
            Assert.AreEqual(418.4f, profile.Kj);
        }

        [TestMethod]
        public void NutritionalProfile_WithNullBaseUnit()
        {
            // Act
            var profile = new NutritionalProfile(1, DateTimeOffset.Now, 1, 100, null,
                100, 418, 5, 2, 15, 3, 6, 0.5f, 2);

            // Assert
            Assert.IsNull(profile.BaseUnit);
        }

        [TestMethod]
        public void NutritionalProfile_WithEmptyBaseUnit()
        {
            // Arrange
            var profile = new NutritionalProfile();

            // Act
            profile.BaseUnit = string.Empty;

            // Assert
            Assert.AreEqual(string.Empty, profile.BaseUnit);
        }
    }
}
