using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Readers;
using LibraryWeb.Models.Books;
using LibraryWeb.Repository.Mappers;

namespace LibraryWeb.Repository
{
    public class HistoryRepository : AbstractRepository<HistoryModel>
    {
        private string _selectionString;

        private ReaderMapper _readerMapper;
        private HistoryMapper _historyMapper;

        public HistoryRepository()
        {
            this._readerMapper = new ReaderMapper();
            this._historyMapper = new HistoryMapper();
            this._selectionString = @"
SELECT r.Id as ReaderId,r.FullName,r.Email,r.Password,
b.Id as BookId,b.Title,b.Available,b.Total,
h.Id as HistoryId,h.DateTaken,h.DateReturned
FROM Readers r 
LEFT JOIN History h ON r.Id=h.ReaderId
LEFT JOIN Books b ON b.id = h.BookId{0};";
        }

        // Deprecate deletion.
        public override void Delete(HistoryModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public override void Insert(HistoryModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public void Insert(ReaderModel entity, SqlConnection connection)
        {
            entity.History.ForEach(e => HistoryValidation.Validate(e));
            // Insert all history entries in one connection opening.
            var commandText = new StringBuilder("INSERT INTO History (BookId, ReaderId, DateTaken, DateReturned) VALUES ");
            var values = entity.History.Select((e, index)
                => String.Format("(@bookId{0},@readerId{0},@dateTaken{0},@dateReturned{0})", index));
            commandText.Append(String.Join(",", values));
            commandText.Append(";");
            using (SqlCommand command = new SqlCommand(commandText.ToString(), connection))
            {
                for (int i = 0; i < entity.History.Count; i++)
                {
                    command.Parameters.AddWithValue($"@bookId{i}", entity.History[i].Book.Id);
                    command.Parameters.AddWithValue($"@readerId{i}", entity.Id);
                    command.Parameters.AddWithValue($"@dateTaken{i}", entity.History[i].DateTaken);
                    command.Parameters.AddWithValue($"@dateReturned{i}", entity.History[i].DateReturned);
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong history insertion query: {ex.Message}");
                }
            }
        }
        
        public override void Update(HistoryModel history, SqlConnection connection)
        {
            //HistoryValidation.Validate(history);
            string commandText = @"UPDATE History SET BookId=@bookId,ReaderId=@readerId,DateTaken=@dateTaken,DateReturned=@dateReturned WHERE id=@id";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@bookId", history.Book.Id);
                command.Parameters.AddWithValue("@readerId", history.Reader.Id);
                command.Parameters.AddWithValue("@dateTaken", history.DateTaken);
                command.Parameters.AddWithValue("@dateReturned", history.DateReturned);
                command.Parameters.AddWithValue("@id", history.Id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong history single update query: {ex.Message}");
                }
            }
        }

        public List<HistoryModel> Select(List<string> conditions)
        {
            var histories = new List<HistoryModel>();
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
                            histories.Add(this._historyMapper.Map(dataReader));
                        }
                        dataReader.Close();
                    }
                    catch (SqlException ex)
                    {
                        throw new ArgumentException($"Wrong selection readers query: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return histories;
        }
    }
}