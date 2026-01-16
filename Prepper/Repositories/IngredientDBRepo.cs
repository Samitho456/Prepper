using Prepper.DTOs;
using Prepper.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class IngredientDBRepo : IRepositoryDB<Ingredient>
    {
        private readonly Supabase.Client _supabase;
        private readonly Dictionary<string, Expression<Func<Ingredient, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "name", i => i.Name },
                { "createdat", i => i.CreatedAt }
            };

        public IngredientDBRepo(Supabase.Client supabase)
        {
            if (supabase == null)
            {
                throw new ArgumentNullException(nameof(supabase));
            }
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
            return result.Models.FirstOrDefault()!;
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
        public async Task<IEnumerable<Ingredient>> GetAllAsync(string? sortBy = null, bool ascending = false)
        {
            // Base query
            var query = _supabase.From<Ingredient>().Select("*");

            // Apply sorting if sortBy is provided
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Validate sortBy parameter
                if (!sortColumns.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sortBy column: {sortBy}");
                }

                // Determine sort direction
                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                // Apply sorting to the query
                query = query
                    .Select(i => new object[] { i.Id, i.Name, i.CreatedAt })
                    .Order(sortColumn, direction);
            }
            // Execute the query and return the results
            var result = await query.Get();
            return result.Models;
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

        public async Task<IEnumerable<IngredientWithNutritionalProfilesDTO>> GetAllIngredientsWithNutritionalProfilesAsync()
        {
            // Get all ingredients
            var ingredientsResult = await _supabase
                .From<Ingredient>()
                .Select("*")
                .Get();

            var ingredients = ingredientsResult.Models;

            // For each ingredient, get its nutritional profiles and map to DTO
            var ingredientDtos = new List<IngredientWithNutritionalProfilesDTO>();
            foreach (var ingredient in ingredients)
            {
                var profilesResult = await _supabase
                    .From<NutritionalProfile>()
                    .Where(np => np.IngredientId == ingredient.Id)
                    .Get();

                var profileDtos = profilesResult.Models
                    .Select(np => new NutritionalProfileDTO
                    {
                        Id = np.Id,
                        CreatedAt = np.CreatedAt,
                        IngredientId = np.IngredientId,
                        UnitAmount = np.UnitAmount,
                        BaseUnit = np.BaseUnit,
                        Kcal = np.Kcal,
                        Kj = np.Kj,
                        FatTotal = np.FatTotal,
                        FatSaturated = np.FatSaturated,
                        CarbohydrateTotal = np.CarbohydrateTotal,
                        CarbohydrateSugars = np.CarbohydrateSugars,
                        Fiber = np.Fiber,
                        Protein = np.Protein,
                        Salt = np.Salt
                    })
                    .ToList();

                ingredientDtos.Add(new IngredientWithNutritionalProfilesDTO
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    NutritionalProfiles = profileDtos
                });
            }

            // Return all ingredients with their nutritional profiles
            return ingredientDtos;
        }

        /// <summary>
        /// Asynchronously retrieves all nutritional profiles associated with the specified ingredient ID.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient for which to retrieve nutritional profiles.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of nutritional
        /// profiles for the specified ingredient. The collection will be empty if no profiles are found.</returns>
        public async Task<IEnumerable<NutritionalProfile?>> GetNutritionalProfilesByIdAsync(int id)
        {
            // Query the database for the nutritional profile with the specified ID
            var result = await _supabase
                .From<NutritionalProfile>()
                .Where(np => np.IngredientId == id)
                .Get();
            // Return the found nutritional profile or null if not found
            return result.Models;
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
            if (result == null || result.Models.Count == 0)
            {
                return null;
            }

            // Return the updated ingredient
            return result.Models.FirstOrDefault()!;
        }
    }
}
