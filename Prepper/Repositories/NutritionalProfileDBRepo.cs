using Prepper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.Repositories
{
    public class NutritionalProfileDBRepo : IRepositoryDB<NutritionalProfile>
    {
        private readonly Supabase.Client _supabase;
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

        /// <summary>
        /// Asynchronously retrieves all nutritional profiles from the data source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all <see
        /// cref="NutritionalProfile"/> objects. The collection is empty if no profiles are found.</returns>
        public async Task<IEnumerable<NutritionalProfile>> GetAllAsync(string sortBy = null, bool ascending = false )
        {
            var result = await _supabase.From<NutritionalProfile>().Get();
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
            var restult = await _supabase.From<NutritionalProfile>().Where(np => np.Id == id).Update(item);
            if(restult.Models.Count == 0)
            {
                return null;
            }
            return restult.Models.FirstOrDefault();
        }
    }
}
