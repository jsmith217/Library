using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Readers;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Books;
using LibraryWeb.Repository.Mappers;

namespace LibraryWeb.Repository
{
    public class ReadersRepository : AbstractRepository<ReaderModel>
    {
        private ReaderMapper _readerMapper;

        public ReadersRepository()
        {
            this._readerMapper = new ReaderMapper();
        }

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

        public List<ReaderModel> LightWeightSelect(List<string> conditions)
        {
            var readers = new List<ReaderModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string conditionText = conditions == null || conditions.Count == 0
                    ? "" : String.Concat(" WHERE ", String.Join(" AND ", conditions));
                string commandText = String.Format("SELECT * FROM Readers{0};", conditionText);
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            readers.Add(this._readerMapper.Map(dataReader));
                        }
                        dataReader.Close();
                    }
                    catch (SqlException ex)
                    {
                        throw new ArgumentException($"Wrong select readers query: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return readers;
        }
    }
}