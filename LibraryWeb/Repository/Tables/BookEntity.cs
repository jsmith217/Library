using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Books;

namespace LibraryWeb.Repository
{
    public class BookEntity : IRepository<BookModel>, ICommonQuery<BookModel>
    {
        public void Delete(BookModel entity)
        {
            throw new NotImplementedException();
        }

        public List<BookModel> GetAll()
        {
            List<BookModel> books = new List<BookModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string commandText = $"SELECT * FROM Books;";
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                books.Add(new BookModel
                                {
                                    Id = int.Parse(reader["Id"].ToString()),
                                    AvailableQuantity = int.Parse(reader["Available"].ToString()),
                                    TotalQuantity = int.Parse(reader["Total"].ToString()),
                                    Title = reader["Title"].ToString()
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return books;
        }

        public BookModel GetById(int id)
        {
            BookModel book = null;
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string commandText = $"SELECT * FROM Books WHERE id = @id;";
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            book = new BookModel
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                AvailableQuantity = int.Parse(reader["Available"].ToString()),
                                TotalQuantity = int.Parse(reader["Total"].ToString()),
                                Title = reader["Title"].ToString()
                            };
                        }
                    }
                    catch (SqlException ex)
                    {
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return book;
        }

        public void Insert(BookModel entity)
        {
            BookValidator.Validate(entity);
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                string commandText = $"INSERT INTO Books (Title, Total, Available) VALUES (@title, @total, @available);";
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@title", entity.Title);
                    command.Parameters.AddWithValue("@total", entity.TotalQuantity);
                    command.Parameters.AddWithValue("@available", entity.AvailableQuantity);

                    try
                    {
                        connection.Open();
                        int recordsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void Update(BookModel entity)
        {
            throw new NotImplementedException();
        }
    }
}