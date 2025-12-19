using System;
using System.Collections.Generic;
using System.Text;

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

        public Ingredient? Delete(int id)
        {
            var ingredient = GetById(id);
            if(ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }
            _repository.Remove(ingredient);
            return ingredient;
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return new List<Ingredient>(_repository);
        }

        public Ingredient? GetById(int id)
        {
            var ingredient = _repository.Find(i => i.Id == id);
            if(ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }
            return ingredient;
        }

        public Ingredient? Update(int id, Ingredient item)
        {
            var existingIngredient = GetById(id);
            if(existingIngredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }
            existingIngredient.Name = item.Name;
            existingIngredient.BaseUnit = item.BaseUnit;
            existingIngredient.NutritionalProfile = item.NutritionalProfile;
            return existingIngredient;
        }
    }
}
