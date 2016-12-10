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
    public class BookService
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
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
               try
                {
                    connection.Open();
                    // add book to books table.
                    this._booksRepo.Insert(book, connection);
                    // add book to author relation
                    this._booksAuthorsRepo.Insert(
                        book.Authors.Select(
                            a => new BooksAuthorsRelationTable { BookId = book.Id, AuthorId = a.Id }).ToList(),
                        connection);
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Connection error");
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
        public void Delete(BookModel book)
        {
            BookModel bookToDelete = new BookModel
            {
                Id = book.Id,
                Authors = book.Authors,
                Title = book.Title,
                TotalQuantity = 0,
                AvailableQuantity = 0
            };
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    this._booksRepo.Update(bookToDelete, connection);
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
                    book.Authors.ForEach(
                        a => this._booksAuthorsRepo.Update(
                            new BooksAuthorsRelationTable { BookId = book.Id, AuthorId=a.Id },
                            connection));
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
            List<BookModel> books = new List<BookModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    books = this._booksRepo.GetAllBooks(connection, orderColumn);
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return books;
        }
    }
}