using Prepper;
using Prepper.Models;

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
            _repo.Add(new Ingredient(1, "Carrot", DateTime.Now));
            _repo.Add(new Ingredient(1, "Potato", DateTime.Now.AddHours(5)));
        }

        [TestMethod]
        public void AddIngredientToRepo()
        {
            var ingredient = new Ingredient(1, "Test Ingredient", DateTime.Now);
            var addedIngredient = _repo.Add(ingredient);
            Assert.IsNotNull(addedIngredient);
            Assert.AreEqual(ingredient.Id, addedIngredient.Id);
            Assert.AreEqual(ingredient.Name, addedIngredient.Name);
        }

        [TestMethod]
        public void GetAllIngredients()
        {
            var allIngredients = _repo.GetAll().ToList();
            Assert.AreEqual(2, allIngredients.Count);
            Assert.AreEqual("Carrot", allIngredients[0].Name);
            Assert.AreEqual("Potato", allIngredients[1].Name);
        }

        [TestMethod]
        public void GetIngredientById()
        {
            var ingredient = _repo.GetById(1);
            Assert.IsNotNull(ingredient);
            Assert.AreEqual(1, ingredient.Id);
            Assert.AreEqual("Carrot", ingredient.Name);
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
            Assert.AreEqual("Potato", deletedIngredient.Name);
            Assert.AreEqual(1, _repo.GetAll().Count());
        }
        [TestMethod]
        public void DeleteIngredient_NotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => _repo.Delete(999));
        }
        [TestMethod]
        public void UpdateIngredient()
        {
            var updatedIngredient = _repo.Update(2, new Ingredient(3, "Updated Ingredient", DateTime.Now));
            Assert.IsNotNull(updatedIngredient);
            Assert.AreEqual(2, updatedIngredient.Id);
            Assert.AreEqual("Updated Ingredient", updatedIngredient.Name);
        }

        [TestMethod]
        public void UpdateIngredient_NotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => _repo.Update(999, new Ingredient(999, "Non-existent Ingredient", DateTime.Now)));
        }

        [TestMethod]
        public void GetSortedByNameIngredients()
        {
        }

        [TestMethod]
        public void GetSortedIngredientsByNutritionalValue()
        {
        }

        [TestMethod]
        public void SearchIngredients()
        {
        }
    }
}
