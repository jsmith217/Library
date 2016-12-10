using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Readers;

namespace LibraryWeb.Repository
{
    public class ReadersRepository : AbstractRepository<ReaderModel>
    {
        #region Write
        // Deprecate deletion.
        public override void Delete(ReaderModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public override void Insert(ReaderModel entity, SqlConnection connection)
        {
            ReaderValidation.Validate(entity);
            string commandText = $"INSERT INTO Readers(Email, FullName, Password) VALUES(@email, @fullName, @password);";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@email", entity.Email);
                command.Parameters.AddWithValue("@fullName", entity.FullName);
                command.Parameters.AddWithValue("@password", entity.Password);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong reader's insert query: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public override void Update(ReaderModel entity, SqlConnection connection)
        {
            ReaderValidation.Validate(entity);
            string commandText = $"UPDATE Readers SET FullName=@fullName, Email=@email, Password=@password WHERE id=@id";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@fullName", entity.FullName);
                command.Parameters.AddWithValue("@email", entity.Email);
                command.Parameters.AddWithValue("@password", entity.Password);
                command.Parameters.AddWithValue("@id", entity.Id);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong reader's update query: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion
    }
}