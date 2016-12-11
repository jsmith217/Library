﻿using System;
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
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
               try
                {
                    connection.Open();
                    // add book to books table.
                    this._booksRepo.Insert(book, connection);
                    // add book to author relation
                    this._booksAuthorsRepo.Insert(book, connection);
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
            return this._booksRepo.Select(orderColumn, new List<string> { "b.Total > 0" });
        }

        public List<BookModel> GetAllAvailableBooks()
        {
            return this._booksRepo.Select(null, new List<string> { "b.Available > 0", " b.Total > 0" });
        }

        public BookModel GetById(int id)
        {
            return this._booksRepo.Select(null, new List<string> { $"b.Id={id}" }).First();
        }

        public bool IsValidBookForDeletion(int id)
        {
            var book = this.GetById(id);
            // Check if this book is not taken by any reader.
            return book.TotalQuantity == book.AvailableQuantity;
        }
    }
}