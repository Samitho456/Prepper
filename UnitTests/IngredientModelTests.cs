namespace UnitTests;

[TestClass]
public class IngredientModelTests
{
    [TestMethod]
    public void CreateValidIngrediant()
    {
        var nutritionalProfile = new Prepper.NutritionalProfile(100, 418, 0.5f, 0.1f, 20f, 5f, 3f, 0.2f);
        var ingredient = new Prepper.Ingredient(1, "Test Ingredient", "g", nutritionalProfile);
        Assert.AreEqual(1, ingredient.Id);
        Assert.AreEqual("Test Ingredient", ingredient.Name);
        Assert.AreEqual("g", ingredient.BaseUnit);
        Assert.AreEqual(100, ingredient.NutritionalProfile.Kcal);
        Assert.AreEqual(418, ingredient.NutritionalProfile.Kj);
        Assert.AreEqual(0.5f, ingredient.NutritionalProfile.FatTotal);
        Assert.AreEqual(0.1f, ingredient.NutritionalProfile.FatSaturated);
        Assert.AreEqual(20f, ingredient.NutritionalProfile.CarbohydrateTotal);
        Assert.AreEqual(5f, ingredient.NutritionalProfile.CarbohydrateSugars);
        Assert.AreEqual(3f, ingredient.NutritionalProfile.Protein);
        Assert.AreEqual(0.2f, ingredient.NutritionalProfile.Salt);
    }
    [TestMethod]
    public void CreateIngrediant_InvalidName_ThrowsException()
    {
        var nutritionalProfile = new Prepper.NutritionalProfile(100, 418, 0.5f, 0.1f, 20f, 5f, 3f, 0.2f);
        Assert.Throws<ArgumentException>(() => new Prepper.Ingredient(1, "", "g", nutritionalProfile));
        Assert.Throws<ArgumentException>(() => new Prepper.Ingredient(1, "A", "g", nutritionalProfile));
        Assert.Throws<ArgumentException>(() => new Prepper.Ingredient(1, new string('A', 101), "g", nutritionalProfile));
    }
    [TestMethod]
    public void CreateIngrediant_NullNutritionalProfile_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new Prepper.Ingredient(1, "Test Ingredient", "g", null!));
    }
    [TestMethod]
    public void CreateIngrediant_NegativeNutritionalValues_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Prepper.Ingredient(1, "Test Ingredient", "g",
            new Prepper.NutritionalProfile(-100, 418, 0.5f, 0.1f, 20f, 5f, 3f, 0.2f)));
        Assert.Throws<ArgumentException>(() => new Prepper.Ingredient(1, "Test Ingredient", "g",
            new Prepper.NutritionalProfile(100, -418, 0.5f, 0.1f, 20f, 5f, 3f, 0.2f)));
        Assert.Throws<ArgumentException>(() => new Prepper.Ingredient(1, "Test Ingredient", "g",
            new Prepper.NutritionalProfile(100, 418, -0.5f, 0.1f, 20f, 5f, 3f, 0.2f)));
        // Additional checks for other nutritional values can be added similarly
    }
}
