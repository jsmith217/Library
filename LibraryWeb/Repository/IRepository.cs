using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Repository
{
    public interface IRepository<T> where T : class
    {
        void Delete(T entity);

        void Insert(T entity);

        void Update(T entity);
    }
}