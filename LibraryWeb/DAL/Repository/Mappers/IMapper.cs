using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LibraryWeb.Repository.Mappers
{
    public interface IMapper<T>
    {
        T Map(SqlDataReader dataReader);
    }
}
