using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Repository
{
    /// <summary>
    /// Represents book repository for elementary CRUD.
    /// </summary>
    public class BooskRepository : AbstractRepository<BookModel>
    {
        private AuthorsRepository _authorRepo;
        private BooksAuthorsRepository _booksAuthorRepo;

        private string _selectionString = "SELECT b.*, a.* FROM Books B INNER JOIN BooksAuthors ba ON ba.BookId=b.Id INNER JOIN Authors a ON a.Id=ba.AuthorId WHERE b.Total > 0 {0};";

        public BooskRepository()
        {
            this._authorRepo = new AuthorsRepository();
            this._booksAuthorRepo = new BooksAuthorsRepository();
        }

        #region Write
        public override void Delete(BookModel entity, SqlConnection connection)
        {
            string commandText = "DELETE FROM Books WHERE id=@id";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@id", entity.Id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong history delete uquery: {ex.Message}");
                }
            }
        }

        public override void Insert(BookModel entity, SqlConnection connection)
        {
            BookValidator.Validate(entity);
            string commandText = $"INSERT INTO Books (Title, Total, Available) VALUES (@title, @total, @available);";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@title", entity.Title);
                command.Parameters.AddWithValue("@total", entity.TotalQuantity);
                command.Parameters.AddWithValue("@available", entity.AvailableQuantity);

                try
                {
                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong book update query: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Updates book.
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(BookModel entity, SqlConnection connection)
        {
            BookValidator.Validate(entity);
            string commandText = "UPDATE Books SET Title=@title, Total=@total, Available=@available WHERE id=@id;";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@title", entity.Title);
                command.Parameters.AddWithValue("@total", entity.TotalQuantity);
                command.Parameters.AddWithValue("@available", entity.AvailableQuantity);
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
        #endregion

        public List<BookModel> GetAllBooks(SqlConnection connection, string orderColumn)
        {
            List<BookModel> books = new List<BookModel>();
            string commandText = String.IsNullOrEmpty(orderColumn) 
                ? String.Format(this._selectionString, orderColumn)
                : String.Format(this._selectionString, $"ORDER BY {orderColumn}");
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int bookId = Int32.Parse(reader["Id"].ToString());
                    BookModel currentBook = null;
                    if ((currentBook = books.FirstOrDefault(b => b.Id == bookId)) == null)
                    {
                        books.Add(new BookModel
                        {
                            Id = Int32.Parse(reader["Id"].ToString()),
                            Title = reader["Title"].ToString(),
                            TotalQuantity = Int32.Parse(reader["Total"].ToString()),
                            AvailableQuantity = Int32.Parse(reader["Available"].ToString()),
                            Authors = new List<AuthorModel>() { new AuthorModel
                            {
                                Id = Int32.Parse(reader["Id"].ToString()),
                                FullName = reader["FullName"].ToString()
                            }
                        },
                        });
                    }
                    else
                    {
                        currentBook.Authors.Add(new AuthorModel
                        {
                            Id = Int32.Parse(reader["Id"].ToString()),
                            FullName = reader["FullName"].ToString()
                        });
                    }
                }
            }
            
            return books;
        }

        /*protected override BookModel GetEntity(SqlDataReader reader, SqlConnection connection)
        {
            int bookId = Int32.Parse(reader["Id"].ToString());
            var authorIds = this._booksAuthorRepo.Select($"SELECT * FROM BooksAuthor WHERE id={bookId};", connection);
            return new BookModel
            {
                Id = bookId,
                Title = reader["Title"].ToString(),
                TotalQuantity = Int32.Parse(reader["Total"].ToString()),
                AvailableQuantity = Int32.Parse(reader["Available"].ToString()),
                Authors = this._authorRepo.Select(
                    $"SELECT * FROM Authors WHERE id in ({String.Join(",", authorIds.Select(x => x.AuthorId))})",
                    connection)
                    .ToList()
            };
        }*/
    }
}