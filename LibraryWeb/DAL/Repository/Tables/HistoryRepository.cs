using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Readers;

namespace LibraryWeb.Repository
{
    public class HistoryRepository : AbstractRepository<HistoryModel>
    {
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
                command.Parameters.AddWithValue("@readerId", history.ReaderId);
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

        /*protected override HistoryRelationTable GetEntity(SqlDataReader reader, SqlConnection connection)
        {
            return new HistoryRelationTable
            {
                Id = Int32.Parse(reader["Id"].ToString()),
                BookId = Int32.Parse(reader["BookId"].ToString()),
                ReaderId = Int32.Parse(reader["ReaderId"].ToString()),
                DateReturned = DateTime.Parse(reader["DateReturned"].ToString()),
                DateTaken = DateTime.Parse(reader["DateTaken"].ToString()),
                Quantity = Int16.Parse(reader["Quantity"].ToString())
            };
        }*/
    }
}