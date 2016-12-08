using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryRepository
{
    public interface IDataBaseEntity<T> where T : class
    {
        int Id { get; }
        
        void Delete(T entity);

        void Insert(T entity);

        void Update(T entity);
    }
}
