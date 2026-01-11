using Prepper.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class MealPlanDBRepo : IRepositoryDB<MealPlan>
    {
        private readonly Supabase.Client _supabase;

        private static readonly Dictionary<string, Expression<Func<MealPlan, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "userid", mp => mp.UserId },
                { "recipeid", mp => mp.RecipeId },
                { "mealtype", mp => mp.MealType },
                { "date", mp => mp.Date },
                { "isconsumed", mp => mp.IsConsumed },
                { "createdat", mp => mp.CreatedAt }
            };

        /// <summary>
        /// Initializes a new instance of the MealPlanDBRepo class using the specified Supabase client.
        /// </summary>
        /// <param name="supabase">The Supabase client instance used to interact with the database. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="supabase"/> is null.</exception>
        public MealPlanDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

        /// <summary>
        /// Asynchronously adds a new meal plan to the database.
        /// </summary>
        /// <param name="item">The meal plan to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added meal plan, or null if the
        /// operation did not succeed.</returns>
        public async Task<MealPlan> AddAsync(MealPlan item)
        {
            var result = await _supabase
                .From<MealPlan>()
                .Insert(item);

            return result.Models.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the meal plan with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to delete. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted meal plan if found and
        /// deleted; otherwise, <see langword="null"/> if no meal plan with the specified identifier exists.</returns>
        public async Task<MealPlan?> DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);

            if (result == null)
            {
                return null;
            }

            await _supabase
                .From<MealPlan>()
                .Where(mp => mp.Id == id)
                .Delete();

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all meal plans from the data source, optionally sorted by the specified column and
        /// direction.
        /// </summary>
        /// <param name="sortBy">The name of the column to sort the results by. Valid values are "userid", "recipeid", "mealtype", "date", "isconsumed", and
        /// "createdat". If null or empty, results are not sorted.</param>
        /// <param name="ascending">Specifies the sort direction. Set to <see langword="true"/> to sort in ascending order; otherwise, results
        /// are sorted in descending order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
        /// cref="MealPlan"/> objects.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sortBy"/> is not null or empty and does not match a valid column name.</exception>
        public async Task<IEnumerable<MealPlan>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            var query = _supabase.From<MealPlan>()
                .Select("*");

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (!sortColumns.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sortBy parameters: {sortBy}. " +
                        $"Possible parameters: userid, recipeid, mealtype, date, isconsumed, createdat");
                }

                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                query = query
                    .Select(mp => new object[] { mp.Id, mp.CreatedAt, mp.IsConsumed, mp.UserId, mp.RecipeId, mp.MealType, mp.Date })
                    .Order(sortColumn, direction);
            }

            var result = await query.Get();

            return result.Models;
        }

        /// <summary>
        /// Asynchronously retrieves a meal plan by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the meal plan with the specified
        /// identifier, or <see langword="null"/> if no matching meal plan is found.</returns>
        public async Task<MealPlan?> GetByIdAsync(int id)
        {
            var result = await _supabase
                .From<MealPlan>()
                .Where(mp => mp.Id == id)
                .Single();

            return result;
        }

        /// <summary>
        /// Updates an existing meal plan with the specified values asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to update.</param>
        /// <param name="item">The meal plan object containing the updated values. All required fields must be set. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated meal plan if the update
        /// is successful; otherwise, <see langword="null"/> if no meal plan with the specified identifier exists.</returns>
        public async Task<MealPlan?> UpdateAsync(int id, MealPlan item)
        {
            var result = await _supabase
                .From<MealPlan>()
                .Where(mp => mp.Id == id)
                .Update(item);

            if (result == null || result.Models.Count == 0)
            {
                return null;
            }

            return result.Models.FirstOrDefault();
        }
    }
}
