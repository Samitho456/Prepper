using Prepper.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class RecipeDBRepo : IRepositoryDB<Recipe>
    {
        // Supabase client instance for database operations
        private readonly Supabase.Client _supabase;

        private static readonly Dictionary<string, Expression<Func<Recipe, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "servings", r => r.Servings  },
                { "meal_type", r => r.MealType },
                { "title", r => r.Title },
                { "createdat", r => r.CreatedAt }
            };

        /// <summary>
        /// Initializes a new instance of the RecipeDBRepo class using the specified Supabase client.
        /// </summary>
        /// <param name="supabase">The Supabase client instance used to interact with the database. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="supabase"/> is null.</exception>
        public RecipeDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

        /// <summary>
        /// Asynchronously adds a new recipe to the database.
        /// </summary>
        /// <param name="item">The recipe to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added recipe, or null if the
        /// operation did not succeed.</returns>
        public async Task<Recipe> AddAsync(Recipe item)
        {
            var result = await _supabase
                .From<Recipe>()
                .Insert(item);

            return result.Models.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the recipe with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to delete. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted recipe if found and
        /// deleted; otherwise, <see langword="null"/> if no recipe with the specified identifier exists.</returns>
        public async Task<Recipe?> DeleteAsync(int id)
        {
            // Get the original object
            var result = await GetByIdAsync(id);

            // Returns null if no object is found
            if (result == null)
            {
                return null;
            }

            // Delete the row in Supabase
            await _supabase
                .From<Recipe>()
                .Where(r => r.Id == id)
                .Delete();

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all recipes from the data source, optionally sorted by the specified column and
        /// direction.
        /// </summary>
        /// <param name="sortBy">The name of the column to sort the results by. Valid values are "servings", "meal_type", "title", and
        /// "createdat". If null or empty, results are not sorted.</param>
        /// <param name="ascending">Specifies the sort direction. Set to <see langword="true"/> to sort in ascending order; otherwise, results
        /// are sorted in descending order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
        /// cref="Recipe"/> objects.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sortBy"/> is not null or empty and does not match a valid column name.</exception>
        public async Task<IEnumerable<Recipe>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            var query = _supabase.From<Recipe>()
                .Select("*");

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (!sortColumns.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sortBy parameters: {sortBy}. " +
                        $"Possible parameters: servings, meal_type, title, createdat");
                }

                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                query = query
                    .Select(r => new object[] { r.Id, r.Title, r.Description, r.Servings, r.MealType, r.PreparationTimeMinutes, r.CreatedAt })
                    .Order(sortColumn, direction);
            }

            var result = await query
                .Get();

            return result
                .Models;
        }

        /// <summary>
        /// Asynchronously retrieves a recipe by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the recipe with the specified
        /// identifier, or <see langword="null"/> if no matching recipe is found.</returns>
        public async Task<Recipe?> GetByIdAsync(int id)
        {
            var result = await _supabase
                .From<Recipe>()
                .Where(r => r.Id == id)
                .Single();

            return result;
        }

        /// <summary>
        /// Updates an existing recipe with the specified values asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to update.</param>
        /// <param name="item">The recipe object containing the updated values. All required fields must be set. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated recipe if the update
        /// is successful; otherwise, <see langword="null"/> if no recipe with the specified identifier exists.</returns>
        public async Task<Recipe?> UpdateAsync(int id, Recipe item)
        {
            // Updates the row in Supabase
            var result = await _supabase
                .From<Recipe>()
                .Where(r => r.Id == id)
                .Update(item);

            // Returns null if the item does not exist
            if (result == null || result.Models.Count == 0)
            {
                return null;
            }

            return result.Models.FirstOrDefault();
        }
    }
}