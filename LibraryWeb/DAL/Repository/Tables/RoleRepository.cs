using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Roles;

namespace LibraryWeb.Repository
{
    public class RoleRepository : IRepository<RoleModel>
    {
        public void Delete(RoleModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public void Insert(RoleModel entity, SqlConnection connection)
        {
            string commandText = "INSERT INTO Roles (RoleName) values (@name);";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong role query: {ex.Message}");
                }
            }
        }

        public void Update(RoleModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public List<RoleModel> Select(List<string> conditions)
        {
            var roles = new List<RoleModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string conditionText = conditions == null || conditions.Count == 0
                    ? "" : String.Concat(" WHERE ", String.Join(" AND ", conditions));
                string commandText = String.Format("SELECT * FROM RoleData{0};", conditionText);
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            roles.Add(new RoleModel {
                                Id = Int32.Parse(dataReader["Id"].ToString()),
                                RoleName = dataReader["Name"].ToString()
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new ArgumentException($"Wrong select query: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return roles;
        }
    }
}