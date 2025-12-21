using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        T Add(T item);
        T? Update(int id, T item);
        T? Delete(int id);
    }
}
