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
    public class BooksAuthorsRepository : IRepository<BookModel>
    {
        #region Write
        public void Delete(BookModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }
        

        public void Insert(BookModel entities, SqlConnection connection)
        {
            var commandText = new StringBuilder("INSERT INTO BooksAuthors (BookId, AuthorId) VALUES ");
            commandText.Append(String.Join(",", entities.Authors.Select((a, index) => $"(@bookId, @authorId{index})")));
            commandText.Append(";");
            using (SqlCommand command = new SqlCommand(commandText.ToString(), connection))
            {
                for (int i = 0; i < entities.Authors.Count; i++)
                {
                    command.Parameters.AddWithValue("@bookId", entities.Id);
                    command.Parameters.AddWithValue($"@authorId{i}", entities.Authors[i].Id);
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

        public void Update(BookModel entity, SqlConnection connection)
        {
            entity.Authors.ForEach(a => this.Update(new Tuple<int, int>(entity.Id, a.Id), connection));
        }

        private void Update(Tuple<int, int> entity, SqlConnection connection)
        {
            string commandText = "UPDATE BooksAuthors SET AuthorId=@authorId WHERE BookId=@bookId;";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@authorId", entity.Item1);
                command.Parameters.AddWithValue("@bookId", entity.Item2);
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