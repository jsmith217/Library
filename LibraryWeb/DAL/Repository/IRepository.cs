using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace LibraryWeb.Repository
{
    public interface IRepository<T> where T : class
    {
        void Delete(T entity, SqlConnection connection);

        void Insert(T entity, SqlConnection connection);

        void Update(T entity, SqlConnection connection);
    }
}