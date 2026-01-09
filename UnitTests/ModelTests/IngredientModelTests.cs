using Prepper.Models;
namespace UnitTests.ModelTests
{
    [TestClass]
    public class IngredientModelTests
    {
        [TestMethod]
        public void CreateValidIngrediant()
        {
            var ingredient = new Ingredient(1, "Test Ingredient", DateTimeOffset.Now);
            Assert.AreEqual(1, ingredient.Id);
            Assert.AreEqual("Test Ingredient", ingredient.Name);
        }
        [TestMethod]
        public void CreateIngrediant_InvalidName_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Ingredient(1, "", DateTimeOffset.Now));
            Assert.Throws<ArgumentException>(() => new Ingredient(1, "A", DateTimeOffset.Now));
            Assert.Throws<ArgumentException>(() => new Ingredient(1, new string('A', 101), DateTimeOffset.Now));
        }
    }
}