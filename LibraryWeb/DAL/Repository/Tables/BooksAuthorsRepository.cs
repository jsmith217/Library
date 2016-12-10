using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Repository
{
    /// <summary>
    /// Responsible for books to authors relationship.
    /// </summary>
    public class BooksAuthorsRepository : AbstractRepository<BooksAuthorsRelationTable>
    {
        #region Write
        public override void Delete(BooksAuthorsRelationTable entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }
        
        public override void Insert(BooksAuthorsRelationTable entities, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public void Insert(List<BooksAuthorsRelationTable> entities, SqlConnection connection)
        {
            var commandText = new StringBuilder("INSERT INTO BooksAuthors (BookId, AuthorId) VALUES ");
            commandText.Append(String.Join(",", entities.Select((a, index) => $"(@bookId, @authorId{index})")));
            commandText.Append(";");
            using (SqlCommand command = new SqlCommand(commandText.ToString(), connection))
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    command.Parameters.AddWithValue("@bookId", entities[i].BookId);
                    command.Parameters.AddWithValue($"@authorId{i}", entities[i].AuthorId);
                }
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong AuthorsBooks insert query: {ex.Message}");
                }
            }
        }

        public override void Update(BooksAuthorsRelationTable entity, SqlConnection connection)
        {
            string commandText = "UPDATE BooksAuthors SET AuthorId=@authorId WHERE BookId=@bookId;";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@authorId", entity.AuthorId);
                command.Parameters.AddWithValue("@bookId", entity.Id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong AuthorsBooks udate query: {ex.Message}");
                }
            }
        }
        #endregion

       /* protected override BooksAuthorsRelationTable GetEntity(SqlDataReader reader, SqlConnection connection)
        {
            return new BooksAuthorsRelationTable
            {
                Id = Int32.Parse(reader["Id"].ToString()),
                BookId = Int32.Parse(reader["BookId"].ToString()),
                AuthorId = Int32.Parse(reader["AuthorId"].ToString())
            };
        }*/
    }
}