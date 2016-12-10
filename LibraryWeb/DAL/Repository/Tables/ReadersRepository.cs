using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Readers;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Books;

namespace LibraryWeb.Repository
{
    public class ReadersRepository : AbstractRepository<ReaderModel>
    {
        private string _selectionString = @"
SELECT r.Id,r.FullName,r.Email,r.Password,b.Id as BookId,b.Title,h.Id as HistoryId,h.DateTaken,h.DateReturned
FROM Readers r 
LEFT JOIN History h ON r.Id=h.ReaderId
LEFT JOIN Books b ON b.id = h.BookId{0};";

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

        public List<ReaderModel> LightWeightSelect()
        {
            var readers = new List<ReaderModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string commandText = "SELECT * FROM Readers";
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            readers.Add(this.MapReader(dataReader));
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

        public List<ReaderModel> Select(List<string> conditions)
        {
            var readers = new List<ReaderModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string conditionsText = conditions == null || conditions.Count == 0
                    ? "" : " WHERE " + String.Join(" AND ", conditions);
                string commandText = String.Format(this._selectionString, conditionsText);
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            int readerId = Int32.Parse(dataReader["Id"].ToString());
                            ReaderModel currentReader = null;
                            if ((currentReader = readers.FirstOrDefault(r => r.Id == readerId)) == null)
                            {
                                var newReader = this.MapReader(dataReader);
                                newReader.History.Add(this.MapHistory(dataReader));
                                readers.Add(newReader);
                            }
                            else
                            {
                                currentReader.History.Add(this.MapHistory(dataReader));
                            }
                        }
                        dataReader.Close();
                    }
                    catch (SqlException ex)
                    {
                        throw new ArgumentException($"Wronf selection readers query: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                } 
            }
            return readers;
        }

        private ReaderModel MapReader(SqlDataReader dataReader)
        {
            return new ReaderModel
            {
                Id = Int32.Parse(dataReader["Id"].ToString()),
                FullName = dataReader["FullName"].ToString(),
                Email = dataReader["Email"].ToString(),
                Password = dataReader["Password"].ToString(),
                History = new List<HistoryModel>()
            };
        }

        private HistoryModel MapHistory(SqlDataReader reader)
        {
            return new HistoryModel
            {
                Id = Int32.Parse(reader["HistoryId"].ToString()),
                DateTaken = DateTime.Parse(reader["DateTaken"].ToString()),
                DateReturned = DateTime.Parse(reader["DateReturned"].ToString()),
                Book = this.MapBook(reader)
            };
        }

        private BookModel MapBook(SqlDataReader reader)
        {
            return new BookModel
            {
                Id = Int32.Parse(reader["BookId"].ToString()),
                Title = reader["Title"].ToString()
            };
        }
    }
}