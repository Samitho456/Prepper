using Prepper.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class RecipeIngredientDBRepo : IRepositoryDB<RecipeIngredients>
    {
        // Dependency injection of Supabase client
        private readonly Supabase.Client _supabase;

        // Mapping of sortable columns for recipe ingredients
        private readonly Dictionary<string, Expression<Func<RecipeIngredients, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "recipeid", ri => ri.RecipeId },
                { "ingredientid", ri => ri.IngredientId },
                { "createdat", ri => ri.CreatedAt }
            };

        // Constructor to initialize the Supabase client
        public RecipeIngredientDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

        /// <summary>
        /// Asynchronously adds a new recipe ingredient to the database.
        /// </summary>
        /// <param name="item">The recipe ingredient to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added recipe ingredient, or
        /// null if the operation did not succeed.</returns>
        public async Task<RecipeIngredients> AddAsync(RecipeIngredients item)
        {
            // Insert the new recipe ingredient into the database
            var result = await _supabase
                .From<RecipeIngredients>()
                .Insert(item);

            return result.Models.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the recipe ingredient with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe ingredient to delete.</param>
        /// <returns>A <see cref="RecipeIngredients"/> object representing the deleted recipe ingredient if found and deleted;
        /// otherwise, <see langword="null"/> if no matching ingredient exists.</returns>
        public async Task<RecipeIngredients?> DeleteAsync(int id)
        {
            // Check if the recipe ingredient exists
            var result = await GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }

            // Delete the recipe ingredient from the database
            await _supabase
                .From<RecipeIngredients>()
                .Where(ri => ri.Id == id)
                .Delete();
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all recipe ingredient records, optionally sorted by the specified column and
        /// direction.
        /// </summary>
        /// <param name="sortBy">The name of the column to sort the results by. If null or empty, no sorting is applied.</param>
        /// <param name="ascending">A value indicating whether to sort the results in ascending order. If <see langword="true"/>, results are
        /// sorted in ascending order; otherwise, in descending order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
        /// cref="RecipeIngredients"/> objects representing all recipe ingredients.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sortBy"/> is specified and does not correspond to a valid sortable column.</exception>
        public async Task<IEnumerable<RecipeIngredients>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            // Start building the query to select all recipe ingredients
            var query = _supabase
                .From<RecipeIngredients>()
                .Select("*");

            // Apply sorting if a sortBy column is specified
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Validate the sortBy column
                if (!sortColumns.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sortBy column: {sortBy}");
                }

                // Determine the sort direction
                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                // Apply the sorting to the query
                query = query.Order(sortColumn, direction);
            }

            // Execute the query and retrieve the results
            var result = await query.Get();
            return result.Models;
        }

        /// <summary>
        /// Asynchronously retrieves the recipe ingredients entry with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe ingredients entry to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see
        /// cref="RecipeIngredients"/> entry if found; otherwise, <see langword="null"/>.</returns>
        public async Task<RecipeIngredients?> GetByIdAsync(int id)
        {
            // Retrieve the recipe ingredients entry by its unique identifier
            var result = await _supabase
                .From<RecipeIngredients>()
                .Where(ri => ri.Id == id)
                .Single();

            return result;
        }

        /// <summary>
        /// Updates the specified recipe ingredients entry with new values asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe ingredients entry to update.</param>
        /// <param name="item">The updated values for the recipe ingredients entry. The item's Id property must match the provided id.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated RecipeIngredients
        /// object if the update is successful; otherwise, null if the entry is not found or the update fails.</returns>
        public async Task<RecipeIngredients?> UpdateAsync(int id, RecipeIngredients item)
        {
            // Check if the recipe ingredient exists first
            var existing = await GetByIdAsync(id);
            if (existing == null)
            {
                return null;
            }

            // Ensure the item's ID matches the provided ID
            item.Id = id;

            // Perform the update
            var result = await _supabase
                .From<RecipeIngredients>()
                .Where(ri => ri.Id == id)
                .Update(item);

            // If no models were returned, the update failed or the item was not found
            if (result == null || result.Models.Count == 0)
            {
                return null;
            }

            return result.Models.FirstOrDefault();
        }
    }
}
