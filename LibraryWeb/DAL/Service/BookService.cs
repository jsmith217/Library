using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Repository;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Service
{
    public class BookService : ICommonQueries<BookModel>
    {
        private BooskRepository _booksRepo;

        private BooksAuthorsRepository _booksAuthorsRepo;

        public BookService()
        {
            this._booksRepo = new BooskRepository();
            this._booksAuthorsRepo = new BooksAuthorsRepository();
        }

        public void Create(BookModel book)
        {
            BookValidator.Validate(book);
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
               try
                {
                    connection.Open();
                    // add book to books table.
                    int id = this._booksRepo.Insert(book, connection);
                    book.Id = id;
                    // add book to author relation
                    this._booksAuthorsRepo.Insert(book, connection);
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Connection error");
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Name of the current book duplicates the name of the existing book.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Deletes books from library. 
        /// The book still remains in the database
        /// yet its quantity is set to 0.
        /// </summary>
        /// <param name="book">Book entity</param>
        public void Delete(int id)
        {
            BookModel book = this.GetById(id);
            if (!this.IsValidBookForDeletion(book))
            {
                throw new ArgumentException("Current book is being read by some reader and thus can't be deleted.");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    this._booksAuthorsRepo.Delete(book, connection);
                    this._booksRepo.Delete(book, connection);
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Connection Error.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void Update(BookModel book)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    this._booksRepo.Update(book, connection);
                    this._booksAuthorsRepo.Update(book, connection);
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Connection Error.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<BookModel> GetAllBooks(string orderColumn)
        {
            return this._booksRepo.Select(orderColumn, "b.Total > 0");
        }

        public List<BookModel> GetAllAvailableBooks()
        {
            return this._booksRepo.Select(null, "b.Available > 0", " b.Total > 0");
        }

        public bool IsAvailable(BookModel model, SqlConnection connection)
        {
            return this._booksRepo.CheckBookIsAvailable(model, connection);
        }

        public BookModel GetById(int id)
        {
            return this._booksRepo.Select(null, $"b.Id={id}").First();
        }

        public void EditQuantity(BookModel editedBook)
        {
            BookModel book = this.GetById(editedBook.Id);
            int booksTaken = book.TotalQuantity - book.AvailableQuantity;
            if (editedBook.TotalQuantity < booksTaken)
            {
                throw new ArgumentException("Total number of the book can't be set smaller than number of books taken by readers.");
            }
            else
            {
                SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString);
                try
                {
                    connection.Open();
                    book.TotalQuantity = editedBook.TotalQuantity;
                    book.AvailableQuantity = editedBook.TotalQuantity - booksTaken;
                    this._booksRepo.Update(book, connection);
                    // update authors
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Connection error.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        
        private bool IsValidBookForDeletion(BookModel book)
        {
            return book.TotalQuantity == book.AvailableQuantity;
        }
    }
}