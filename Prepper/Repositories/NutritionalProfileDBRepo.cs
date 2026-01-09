using Prepper.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class NutritionalProfileDBRepo : IRepositoryDB<NutritionalProfile>
    {
        private readonly Supabase.Client _supabase;

        private static readonly Dictionary<string, Expression<Func<NutritionalProfile, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "kcal", i => i.Kcal },
                { "kj", i => i.Kj },
                { "protein", i => i.Protein },
                { "fat_total", i => i.FatTotal },
                { "fat_saturated", i => i.FatSaturated  },
                { "carbs_total", i => i.CarbohydrateTotal },
                { "carbs_sugars", i => i.CarbohydrateSugars },
                { "fiber", i => i.Fiber },
                { "salt", i => i.Salt },
                { "createdat", i => i.CreatedAt }
            };
        public NutritionalProfileDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

        /// <summary>
        /// Asynchronously adds a new nutritional profile to the data store.
        /// </summary>
        /// <param name="item">The nutritional profile to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous add operation. The task result contains the added nutritional
        /// profile, including any updated fields such as generated identifiers.</returns>
        /// <exception cref="NotImplementedException">The method is not implemented.</exception>
        public async Task<NutritionalProfile> AddAsync(NutritionalProfile item)
        {
            var result = await _supabase.From<NutritionalProfile>().Insert(item);
            return result.Models.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the nutritional profile with the specified identifier and returns the deleted profile if found.
        /// </summary>
        /// <param name="id">The unique identifier of the nutritional profile to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted nutritional profile
        /// if the profile was found; otherwise, the method throws an exception.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a nutritional profile with the specified identifier does not exist.</exception>
        public async Task<NutritionalProfile?> DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }
            await _supabase.From<NutritionalProfile>().Where(np => np.Id == id).Delete();

            return result;
        }

        public async Task<IEnumerable<NutritionalProfile>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            // base query. 
            var query = _supabase.From<NutritionalProfile>().Select("*");

            // Apply sorting if sortBy is provided
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Validate sortBy parameter
                if (!sortColumns.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sortBy parametres: {sortBy}. " +
                        $"Possible parametres: kcal, kj, protein, fat_total, fat_saturated, carbs_total, carbs_sugars, fiber, salt");
                }

                // Determine sort direction
                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                // Apply sorting to the query
                query = query
                    .Select(i => new object[] { i.Id, i.UnitAmount, i.BaseUnit, i.Kcal, i.Kj, i.Protein, i.FatTotal, i.FatSaturated, i.CarbohydrateTotal, i.CarbohydrateSugars, i.Fiber, i.Salt })
                    .Order(sortColumn, direction);
            }

            var result = await query.Get();
            return result.Models;
        }

        /// <summary>
        /// Retrieves a nutritional profile by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the nutritional profile to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the nutritional profile with the
        /// specified identifier, or <see langword="null"/> if no matching profile is found.</returns>
        public async Task<NutritionalProfile?> GetByIdAsync(int id)
        {
            var result = await _supabase
                .From<NutritionalProfile>()
                .Where(np => np.Id == id)
                .Single();

            return result;
        }

        /// <summary>
        /// Updates the nutritional profile with the specified identifier using the provided data.
        /// </summary>
        /// <param name="id">The unique identifier of the nutritional profile to update.</param>
        /// <param name="item">The updated nutritional profile data to apply. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated nutritional profile
        /// if the update is successful; otherwise, null.</returns>
        public async Task<NutritionalProfile?> UpdateAsync(int id, NutritionalProfile item)
        {
            var result = await _supabase.From<NutritionalProfile>().Where(np => np.Id == id).Update(item);
            if (result == null || result.Models.Count == 0)
            {
                return null;
            }
            return result.Models.FirstOrDefault();
        }
    }
}
