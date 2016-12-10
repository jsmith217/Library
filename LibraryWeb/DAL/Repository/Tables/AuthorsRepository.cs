using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Repository
{
    public class AuthorsRepository : AbstractRepository<AuthorModel>
    {
        // Deprecate deletion.
        public override void Delete(AuthorModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public override void Insert(AuthorModel entity, SqlConnection connection)
        {
            string commandText = $"INSERT INTO Authors (FullName) VALUES @fullName";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@fullName", entity.FullName);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong author insert query: {ex.Message}");
                }
            }
        }

        public override void Update(AuthorModel entity, SqlConnection connection)
        {
            string commandText = "UPDATE Authors SET FullName = @fullName WHERE id=@id;";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@fullName", entity.FullName);
                command.Parameters.AddWithValue("@id", entity.Id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong author update query: {ex.Message}");
                }
            }
        }
        
        /*protected override AuthorModel GetEntity(SqlDataReader reader, SqlConnection connection)
        {
            return new AuthorModel
            {
                Id = Int32.Parse(reader["Id"].ToString()),
                FullName = reader["FullName"].ToString()
            };
        }*/
    }
}