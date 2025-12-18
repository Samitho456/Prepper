using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public T? GetById(int id);
        public T Add(T item);
        public T? Update(int id, T item);
        public T? Delete(int id);
    }
}
