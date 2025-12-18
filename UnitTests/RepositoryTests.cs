using Prepper;

namespace UnitTests
{
    [TestClass]
    public sealed class RepositoryTests
    {
        private IngrediantRepo _repo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new IngrediantRepo();
            _repo.Add(new Ingredient(0, "Ingredient 1", UnitEnum.Unit.Gram, new NutritionalProfile(50, 200, 0.2f, 0.05f, 10f, 2f, 1f, 0.1f)));
            _repo.Add(new Ingredient(0, "Ingredient 2", UnitEnum.Unit.Milliliter, new NutritionalProfile(30, 125, 0.1f, 0.02f, 5f, 1f, 0.5f, 0.05f)));
            _repo.Add(new Ingredient(0, "Ingredient 3", UnitEnum.Unit.Piece, new NutritionalProfile(80, 335, 0.3f, 0.08f, 15f, 3f, 2f, 0.15f)));
        }

        [TestMethod]
        public void AddIngredientToRepo()
        {
            var nutritionalProfile = new NutritionalProfile(100, 418, 0.5f, 0.1f, 20f, 5f, 3f, 0.2f);
            var ingredient = new Ingredient(1, "Test Ingredient", UnitEnum.Unit.Gram, nutritionalProfile);
            var addedIngredient = _repo.Add(ingredient);
            Assert.IsNotNull(addedIngredient);
            Assert.AreEqual(ingredient.Id, addedIngredient.Id);
            Assert.AreEqual(ingredient.Name, addedIngredient.Name);
            Assert.AreEqual(ingredient.BaseUnit, addedIngredient.BaseUnit);
            Assert.AreEqual(ingredient.NutritionalProfile.Kcal, addedIngredient.NutritionalProfile.Kcal);
        }

        [TestMethod]
        public void GetAllIngredients()
        {
            var allIngredients = _repo.GetAll().ToList();
            Assert.AreEqual(3, allIngredients.Count);
            Assert.AreEqual("Ingredient 1", allIngredients[0].Name);
            Assert.AreEqual("Ingredient 2", allIngredients[1].Name);
            Assert.AreEqual("Ingredient 3", allIngredients[2].Name);
        }

        [TestMethod]
        public void GetIngredientById()
        {
            var ingredient = _repo.GetById(1);
            Assert.IsNotNull(ingredient);
            Assert.AreEqual(1, ingredient.Id);
            Assert.AreEqual("Ingredient 1", ingredient.Name);
        }

        [TestMethod]
        public void GetIngredientById_NotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => _repo.GetById(999));
        }

        [TestMethod]
        public void DeleteIngredient()
        {
            var deletedIngredient = _repo.Delete(2);
            Assert.AreEqual(2, deletedIngredient.Id);
            Assert.AreEqual("Ingredient 2", deletedIngredient.Name);
            Assert.AreEqual(2, _repo.GetAll().Count());
        }
        [TestMethod]
        public void DeleteIngredient_NotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => _repo.Delete(999));
        }
    }
}
