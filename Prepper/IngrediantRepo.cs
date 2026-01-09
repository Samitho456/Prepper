using Prepper.Models;

namespace Prepper
{
    public class IngrediantRepo : IRepository<Ingredient>
    {
        private int _nextId = 1;
        private List<Ingredient> _repository;
        public IngrediantRepo()
        {
            _repository = new List<Ingredient>();
        }
        /// <summary>
        /// Adds a new ingredient to the repository and assigns it a unique identifier.
        /// </summary>
        /// <param name="item">The ingredient to add. The object's properties, except for the identifier, should be set before calling this
        /// method.</param>
        /// <returns>The ingredient instance with its identifier property set to a unique value.</returns>
        public Ingredient Add(Ingredient item)
        {
            item.Id = _nextId++;
            _repository.Add(item);
            return item;
        }

        /// <summary>
        /// Deletes the ingredient with the specified identifier from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to delete.</param>
        /// <returns>The deleted ingredient if the operation is successful; otherwise, <see langword="null"/> if the ingredient
        /// does not exist.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no ingredient with the specified <paramref name="id"/> exists.</exception>
        public Ingredient? Delete(int id)
        {
            var ingredient = GetById(id);
            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }
            _repository.Remove(ingredient);
            return ingredient;
        }

        /// <summary>
        /// Retrieves all ingredients from the repository.
        /// </summary>
        /// <returns>An enumerable collection containing all ingredients. The collection will be empty if no ingredients are
        /// present.</returns>
        public IEnumerable<Ingredient> GetAll()
        {
            return new List<Ingredient>(_repository);
        }

        /// <summary>
        /// Retrieves the ingredient with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to retrieve.</param>
        /// <returns>The ingredient that matches the specified identifier.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no ingredient with the specified identifier is found.</exception>
        public Ingredient? GetById(int id)
        {
            var ingredient = _repository.Find(i => i.Id == id);
            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }
            return ingredient;
        }

        /// <summary>
        /// Updates the properties of an existing ingredient with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to update.</param>
        /// <param name="item">An <see cref="Ingredient"/> object containing the updated values. Only the <c>Name</c> property is used.</param>
        /// <returns>The updated <see cref="Ingredient"/> if the operation is successful; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if an ingredient with the specified <paramref name="id"/> does not exist.</exception>
        public Ingredient? Update(int id, Ingredient item)
        {
            var existingIngredient = GetById(id);
            if (existingIngredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }
            existingIngredient.Name = item.Name;
            return existingIngredient;
        }
    }
}
