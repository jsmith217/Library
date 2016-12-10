using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace LibraryWeb.Repository
{
    public abstract class AbstractRepository<T> where T : class
    {
        public abstract void Delete(T entity, SqlConnection connection);

        public abstract void Insert(T entity, SqlConnection connection);

        public abstract void Update(T entity, SqlConnection connection);
    }
}