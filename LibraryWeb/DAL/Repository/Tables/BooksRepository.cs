﻿using System;
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
        private readonly string _selectionString;

        public BooskRepository()
        {
            _selectionString = @"SELECT b.*, a.Id as AuthorId, a.FullName FROM Books b 
LEFT JOIN BooksAuthors ba ON ba.BookId=b.Id 
LEFT JOIN Authors a ON a.Id=ba.AuthorId WHERE b.Total > 0 {1} {0};";
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

        public List<BookModel> GetAllBooks(string orderColumn, List<string> conditions)
        {
            List<BookModel> books = new List<BookModel>();
            string orderText = String.IsNullOrEmpty(orderColumn) ? "" : $"ORDER BY {orderColumn}";
            string conditionsText = conditions == null ? "" : "AND " + String.Join(" AND ", conditions);

            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string commandText = String.Format(this._selectionString, orderText, conditionsText);
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int bookId = Int32.Parse(reader["Id"].ToString());
                            BookModel currentBook = null;
                            if ((currentBook = books.FirstOrDefault(b => b.Id == bookId)) == null)
                            {
                                books.Add(new BookModel {
                                    Id = bookId,
                                    Title = reader["Title"].ToString(),
                                    TotalQuantity = Int32.Parse(reader["Total"].ToString()),
                                    AvailableQuantity = Int32.Parse(reader["Available"].ToString()),
                                    Authors = new List<AuthorModel>()
                                    {
                                        this.MapAuthor(reader)
                                    }
                                });
                            }
                            else
                            {
                                currentBook.Authors.Add(this.MapAuthor(reader));
                            }
                        }
                        reader.Close();
                    }
                    catch (SqlException ex)
                    {
                        throw new ArgumentException($"Wrong select query: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return books;
        }

        private AuthorModel MapAuthor(SqlDataReader reader)
        {
            return new AuthorModel
            {
                Id = Int32.Parse(reader["AuthorId"].ToString()),
                FullName = reader["FullName"].ToString()
            };
        }
    }
}