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
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Repository
{
    public class HistoryRepository : IRepository<HistoryModel>
    {
        private string _selectionString;
        
        private HistoryMapper _historyMapper;

        private ReadCommandBuilder _commandBuilder;

        public HistoryRepository()
        {
            this._historyMapper = new HistoryMapper();
            this._commandBuilder = new ReadCommandBuilder();
            this._selectionString = @"
SELECT r.Id as ReaderId,r.FullName,r.Email,r.Password,
b.Id as BookId,b.Title,b.Available,b.Total,
h.Id as HistoryId,h.DateTaken,h.DateReturned, a.FullName as AuthorName, a.Id as AuthorId
FROM Readers r 
LEFT JOIN History h ON r.Id=h.ReaderId
LEFT JOIN Books b ON b.id = h.BookId
left join BooksAuthors ba on b.Id = ba.BookId
Left join Authors a on a.id = ba.AuthorId";
        }

        // Deprecate deletion.
        public void Delete(HistoryModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public void Insert(HistoryModel entity, SqlConnection connection)
        {
            HistoryValidation.Validate(entity);
            // Insert all history entries in one connection opening.
            var commandText = "INSERT INTO History (BookId, ReaderId, DateTaken, DateReturned) VALUES (@bookId,@readerId,@dateTaken,@dateReturned);";
            
            using (SqlCommand command = new SqlCommand(commandText.ToString(), connection))
            {
                command.Parameters.AddWithValue("@bookId", entity.Book.Id);
                command.Parameters.AddWithValue("@readerId", entity.Id);
                command.Parameters.AddWithValue("@dateTaken", entity.DateTaken);
                command.Parameters.AddWithValue("@dateReturned", (object)entity.DateReturned ?? DBNull.Value);

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
        
        public void Update(HistoryModel history, SqlConnection connection)
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

        public List<HistoryModel> Select(params string[] conditions)
        {
            var histories = new List<HistoryModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                using (SqlCommand command = this._commandBuilder.BuildNotSecureCommand(
                    this._selectionString, connection, "", conditions))
                {
                    try
                    {
                        connection.Open();
                        var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            var history = this._historyMapper.Map(dataReader);
                            HistoryModel historyInList = null;
                            if ((historyInList = histories.FirstOrDefault(h => h.Id == history.Id)) == null)
                            {
                                // Found new history. Update everything.
                                var book = this._historyMapper.MapBook(dataReader);
                                if (book != null)
                                {
                                    var author = this._historyMapper.MapAuthor(dataReader);
                                    if (author != null)
                                    {
                                        book.Authors.Add(author);
                                    }
                                    history.Book = book;
                                }
                                var reader = this._historyMapper.MapReader(dataReader);
                                history.Reader = reader;
                                histories.Add(history);
                            }
                            else
                            {
                                // This history is already present in list.
                                // It means we've found another author of the present book.
                                var author = this._historyMapper.MapAuthor(dataReader);
                                historyInList.Book.Authors.Add(author);
                            }
                            //histories.Add(this._historyMapper.Map(dataReader));
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