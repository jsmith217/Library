using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LibraryWeb.Models.Readers;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Roles;

namespace LibraryWeb.Repository.Mappers
{
    public class ReaderMapper : IMapper<ReaderModel>
    {
        public ReaderModel Map(SqlDataReader dataReader)
        {
            return new ReaderModel
            {
                Id = Int32.Parse(dataReader["Id"].ToString()),
                FullName = dataReader["FullName"].ToString(),
                Email = dataReader["Email"].ToString(),
                Password = dataReader["Password"].ToString(),
                Role = this.MapRole(dataReader),
                History = new List<HistoryModel>()
            };
        }

        public RoleModel MapRole(SqlDataReader dataReader)
        {
            return new RoleModel
            {
                Id = int.Parse(dataReader["RoleId"].ToString()),
                RoleName = dataReader["Name"].ToString()
            };
        }
    }
}