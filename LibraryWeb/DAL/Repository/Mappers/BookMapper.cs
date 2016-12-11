using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Repository.Mappers
{
    public class BookMapper : IMapper<BookModel>
    {
        public BookModel Map(SqlDataReader dataReader)
        {
            return new BookModel
            {
                Id = Int32.Parse(dataReader["Id"].ToString()),
                Title = dataReader["Title"].ToString(),
                TotalQuantity = Int32.Parse(dataReader["Total"].ToString()),
                AvailableQuantity = Int32.Parse(dataReader["Available"].ToString()),
                Authors = new List<AuthorModel>()
            };
        }
        public AuthorModel MapAuthor(SqlDataReader reader)
        {
            return new AuthorModel
            {
                Id = Int32.Parse(reader["AuthorId"].ToString()),
                FullName = reader["FullName"].ToString()
            };
        }
    }
}