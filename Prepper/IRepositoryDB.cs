using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper
{
    public interface IRepositoryDB<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T item);
        Task<T?> UpdateAsync(int id, T item);
        Task<T?> DeleteAsync(int id);
    }
}
