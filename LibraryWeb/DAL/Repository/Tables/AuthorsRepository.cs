using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Repository
{
    public class AuthorsRepository : IRepository<AuthorModel>
    {
        private ReadCommandBuilder _commandBuilder;

        public AuthorsRepository()
        {
            this._commandBuilder = new ReadCommandBuilder();
        }

        // Deprecate deletion.
        public void Delete(AuthorModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public void Insert(AuthorModel entity, SqlConnection connection)
        {
            string commandText = $"INSERT INTO Authors (FullName) VALUES (@fullName)";
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

        public void Update(AuthorModel entity, SqlConnection connection)
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

        public List<AuthorModel> Select(params string[] conditions)
        {
            List<AuthorModel> authors = new List<AuthorModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string commandText = "SELECT * FROM Authors";
                using (SqlCommand command = this._commandBuilder.BuildNotSecureCommand(commandText, connection, "", conditions))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            authors.Add(new AuthorModel
                            {
                                Id = Int32.Parse(reader["Id"].ToString()),
                                FullName = reader["FullName"].ToString()
                            });
                        } 
                    }
                    catch (SqlException ex)
                    {
                        throw new ArgumentException("Internal server error. Connection not established.");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return authors;
        }
    }
}