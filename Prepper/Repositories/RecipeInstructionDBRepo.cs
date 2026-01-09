using Prepper.Models;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class RecipeInstructionDBRepo : IRepositoryDB<RecipeInstruction>
    {
        // Dependency injection of Supabase client
        private readonly Supabase.Client _supabase;

        // Mapping of sortable columns for recipe instructions
        private readonly static Dictionary<string, System.Linq.Expressions.Expression<Func<RecipeInstruction, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "recipeid", ri => ri.RecipeId },
                { "createdat", ri => ri.CreatedAt }
            };
        // Constructor to initialize the Supabase client
        public RecipeInstructionDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

        /// <summary>
        /// Asynchronously adds a new recipe instruction to the data store.
        /// </summary>
        /// <param name="item">The recipe instruction to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added recipe instruction, or
        /// null if the operation did not succeed.</returns>
        public async Task<RecipeInstruction> AddAsync(RecipeInstruction item)
        {
            // Insert the new recipe instruction into the database
            var result = await _supabase
                .From<RecipeInstruction>()
                .Insert(item);

            return result
                .Models
                .FirstOrDefault();
        }

        /// <summary>
        /// Deletes the recipe instruction with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe instruction to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted <see
        /// cref="RecipeInstruction"/> if found and deleted; otherwise, <see langword="null"/>.</returns>
        public async Task<RecipeInstruction?> DeleteAsync(int id)
        {
            // Check if the recipe instruction exists
            var result = await GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }

            // Delete the recipe instruction from the database
            await _supabase
                .From<RecipeInstruction>()
                .Where(ri => ri.Id == id)
                .Delete();

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all recipe instructions, optionally sorted by a specified column and direction.
        /// </summary>
        /// <param name="sortBy">The name of the column to sort the results by. If null or empty, no sorting is applied.</param>
        /// <param name="ascending">true to sort the results in ascending order; otherwise, false to sort in descending order. Ignored if sortBy
        /// is null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all recipe
        /// instructions, optionally sorted as specified.</returns>
        /// <exception cref="ArgumentException">Thrown if sortBy is not null or empty and does not correspond to a valid sortable column.</exception>
        public async Task<IEnumerable<RecipeInstruction>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            var query = _supabase
                .From<RecipeInstruction>()
                .Select("*");

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (!sortColumns.TryGetValue(sortBy, out var sortcolumn))
                {
                    throw new ArgumentException($"Invalid sortBy value: {sortBy}");
                }

                // Determine the sort direction
                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                query = query
                    .Select(ri => new object[] { ri.Id, ri.RecipeId, ri.StepNumber, ri.InstructionText, ri.CreatedAt })
                    .Order(sortcolumn, direction);
            }

            var result = await query.Get();
            return result.Models;
        }

        /// <summary>
        /// Asynchronously retrieves a recipe instruction by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe instruction to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see
        /// cref="RecipeInstruction"/> if found; otherwise, <see langword="null"/>.</returns>
        public Task<RecipeInstruction?> GetByIdAsync(int id)
        {
            var result = _supabase
                .From<RecipeInstruction>()
                .Where(ri => ri.Id == id)
                .Single();

            return result;
        }

        /// <summary>
        /// Asynchronously updates the specified recipe instruction with new values.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe instruction to update.</param>
        /// <param name="item">The updated values for the recipe instruction. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see
        /// cref="RecipeInstruction"/> if the update is successful; otherwise, <see langword="null"/> if no matching
        /// instruction is found.</returns>
        public async Task<RecipeInstruction?> UpdateAsync(int id, RecipeInstruction item)
        {
            // Updates the row in Supabase
            var result = await _supabase
                .From<RecipeInstruction>()
                .Where(ri => ri.Id == id)
                .Update(item);

            if (result == null || result.Models.Count == 0)
            {
                return null;
            }

            return result.Models.FirstOrDefault();
        }
    }
}
