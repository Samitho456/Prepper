using Prepper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class InventoryItemDBRepo(Supabase.Client supabase) : IRepositoryDB<InventoryItem>
    {
        private readonly Supabase.Client _supabase = supabase ?? throw new ArgumentException(nameof(supabase));

        private static readonly Dictionary<string, Func<InventoryItem, object>> _sortOptions = new()
        {
            { "id", item => item.Id },
            { "ingredientid", item => item.IngredientId },
            { "recipeid", item => item.RecipeId },
            { "quantity", item => item.Quantity },
            { "unit", item => item.Unit },
            { "locationid", item => item.LocationId },
            { "expiration-date", item => item.ExpirationDate }
        };

        public async Task<InventoryItem> AddAsync(InventoryItem item)
        {
            var result = await _supabase
                .From<InventoryItem>()
                .Insert(item);
            return result.Models.FirstOrDefault()!;
        }

        public async Task<InventoryItem?> DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }

            await _supabase
                .From<InventoryItem>()
                .Where(i => i.Id == id)
                .Delete();
            return result;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync(string sortBy, bool ascending)
        {
            var query = _supabase.From<InventoryItem>().Select("*");
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if(!_sortOptions.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sort option: {sortBy}");
                }

                var direction = ascending ? Ordering.Ascending : Ordering.Descending;
                query = query.Order(sortBy, direction);
            }

            var result = await query.Get();
            return result.Models;
        }

        public async Task<InventoryItem?> GetByIdAsync(int id)
        {
            var result = await _supabase
                .From<InventoryItem>()
                .Where(i => i.Id == id)
                .Single();

            return result;
        }

        public async Task<InventoryItem?> UpdateAsync(int id, InventoryItem item)
        {
            var result = await _supabase
                .From<InventoryItem>()
                .Where(i => i.Id == id)
                .Update(item);

            if(result == null || result.Models.Count == 0)
            {
                return null;
            }

            return result.Models.First();
        }
    }
}
