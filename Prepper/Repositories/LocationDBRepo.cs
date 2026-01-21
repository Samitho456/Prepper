using Prepper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class LocationDBRepo(Supabase.Client supabase) : IRepositoryDB<Location>
    {
        private readonly Supabase.Client _supabase = supabase ?? throw new ArgumentException(nameof(supabase));
        private static readonly Dictionary<string, Func<Location, object>> _sortOptions = new()
        {
            { "id", location => location.Id },
            { "name", location => location.Name },
            { "createdat", location => location.CreatedAt }
        };

        public async Task<Location> AddAsync(Location item)
        {
            var result = await _supabase
                .From<Location>()
                .Insert(item);
            return result.Models.FirstOrDefault()!;
        }

        public async Task<Location?> DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if(result == null)
            { 
                return null;
            }

            await _supabase
                .From<Location>()
                .Where(l => l.Id == id)
                .Delete();
            return result;

        }

        public async Task<IEnumerable<Location>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            // If no sort option is provided, return all locations without sorting
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                var locations = await _supabase.From<Location>().Select("*").Get();
                return locations.Models;
            }

            // Validate the sort option
            if (_sortOptions.TryGetValue(sortBy, out var sortColumn))
            {
                throw new NotImplementedException($"Invalid sort option: {sortBy}");
            }

            // Fetch and sort the locations based on the provided sort option
            var result = await _supabase
                .From<Location>()
                .Select("*")
                .Order(sortBy, ascending ? Ordering.Ascending : Ordering.Descending)
                .Get();

            return result.Models;
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            var result = await _supabase
                .From<Location>()
                .Where(l => l.Id == id)
                .Single();
            return result;
        }

        public async Task<Location?> UpdateAsync(int id, Location item)
        {
            var result = await _supabase
                .From<Location>()
                .Where(l => l.Id == id)
                .Set(x => x.Name, item.Name)
                .Update();

            if (result == null || result.Models.Count == 0)
            {
                return null;
            }
            return result.Models.FirstOrDefault()!;
        }
    }
}
