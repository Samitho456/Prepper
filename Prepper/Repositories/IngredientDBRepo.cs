using Prepper.Models;
using Supabase;
using Supabase.Postgrest;
using static Supabase.Postgrest.QueryOptions;

namespace Prepper.Repositories
{
    public class IngredientDBRepo : IRepositoryDB<Ingredient>
    {
        private readonly Supabase.Client _supabase;
        public IngredientDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        /// <summary>
        /// Asynchronously adds a new ingredient to the database.
        /// </summary>
        /// <param name="item">The ingredient to add. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the inserted ingredient,
        /// including its assigned ID. Returns null if the insertion fails.</returns>
        public async Task<Ingredient> AddAsync(Ingredient item)
        {
            // Insert the new ingredient into the database
            var result = await _supabase.From<Ingredient>()
                .Insert(item);

            // Return the inserted ingredient with its assigned ID
            return result.Models.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the ingredient with the specified identifier and returns the deleted ingredient.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to delete. Must be a valid identifier for an existing ingredient.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains the deleted ingredient if
        /// the operation succeeds; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if no ingredient with the specified identifier exists.</exception>
        public async Task<Ingredient?> DeleteAsync(int id)
        {
            // Get the ingredient before deleting to return it
            var result = await GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }
            // Delete the ingredient
            await _supabase.From<Ingredient>().Where(i => i.Id == id).Delete();

            // Return the deleted ingredient
            return result;
        }

        /// <summary>
        /// Retrieves all ingredients from the data source asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all
        /// <see cref="Ingredient"/> objects. The collection will be empty if no ingredients are found.</returns>
        public async Task<IEnumerable<Ingredient>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            if(sortBy == null)
            {
                // Retrieve all ingredients from the database
                var result = await _supabase.From<Ingredient>().Get();

                // Return the list of ingredients
                return result.Models;
            }
            // Sort by name of the ingredient
            else if (sortBy.ToLower() == "name")
            {
                // Sorts the names Ascending
                if (ascending)
                {
                    var result = await _supabase.From<Ingredient>()
                        .Select(i => new object[] { i.Id, i.Name })
                        .Order(i => i.Name, Supabase.Postgrest.Constants.Ordering.Ascending)
                        .Get();
                    return result.Models;
                }
                // Sorts the name Descending
                else
                {
                    var result = await _supabase.From<Ingredient>()
                        .Select(i => new object[] { i.Id, i.Name })
                        .Order(i => i.Name, Supabase.Postgrest.Constants.Ordering.Descending)
                        .Get();
                    return result.Models;
                }
            }
            // Sort by time added to supabase
            else if (sortBy.ToLower() == "createdat")
            {
                // Sorts the ingredents by newest first
                if (ascending)
                {
                    var result = await _supabase.From<Ingredient>()
                        .Select(i => new object[] { i.Id, i.Name })
                        .Order(i => i.CreatedAt, Supabase.Postgrest.Constants.Ordering.Ascending)
                        .Get();
                    return result.Models;
                }
                // Sorts the ingredients by oldest first
                else
                {
                    var result = await _supabase.From<Ingredient>()
                        .Select(i => new object[] { i.Id, i.Name })
                        .Order(i => i.CreatedAt, Supabase.Postgrest.Constants.Ordering.Descending)
                        .Get();
                    return result.Models;
                }
            }
            else
            {
                throw new ArgumentException("Invalid sortBy parameter");
            }
        }

        /// <summary>
        /// Asynchronously retrieves an ingredient by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ingredient with the
        /// specified identifier, or <see langword="null"/> if no matching ingredient is found.</returns>
        public async Task<Ingredient?> GetByIdAsync(int id)
        {
            // Query the database for the ingredient with the specified ID
            var result = await _supabase.From<Ingredient>().Where(i => i.Id == id).Single();

            // Return the found ingredient or null if not found
            return result;
        }

        /// <summary>
        /// Updates the ingredient with the specified identifier using the provided values.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to update.</param>
        /// <param name="item">An <see cref="Ingredient"/> object containing the updated values for the ingredient.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see
        /// cref="Ingredient"/> if the update is successful; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if no ingredient with the specified <paramref name="id"/> is found.</exception>
        public async Task<Ingredient?> UpdateAsync(int id, Ingredient item)
        {
            // Update the ingredient with the specified ID
            var result = await _supabase.From<Ingredient>().Where(i => i.Id == id).Update(item);
            if (result.Models.Count == 0 || result == null)
            {
                throw new ArgumentNullException(nameof(result), "Ingredient not found");
            }

            // Return the updated ingredient
            return result.Models.FirstOrDefault();
        }
    }
}
