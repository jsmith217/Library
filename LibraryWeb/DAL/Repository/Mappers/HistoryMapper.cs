using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Readers;

namespace LibraryWeb.Repository.Mappers
{
    public class HistoryMapper : IMapper<HistoryModel>
    {
        public HistoryModel Map(SqlDataReader dataReader)
        {
            return new HistoryModel
            {
                Id = Int32.Parse(dataReader["HistoryId"].ToString()),
                DateTaken = DateTime.Parse(dataReader["DateTaken"].ToString()),
                DateReturned = DateTime.Parse(dataReader["DateReturned"].ToString()),
                Book = this.MapBook(dataReader),
                Reader = this.MapReader(dataReader)
            };
        }

        private BookModel MapBook(SqlDataReader reader)
        {
            return new BookModel
            {
                Id = Int32.Parse(reader["BookId"].ToString()),
                Title = reader["Title"].ToString(),
                AvailableQuantity = Int32.Parse(reader["Available"].ToString()),
                TotalQuantity = Int32.Parse(reader["Total"].ToString())
            };
        }

        private ReaderModel MapReader(SqlDataReader dataReader)
        {
            return new ReaderModel
            {
                Id = Int32.Parse(dataReader["ReaderId"].ToString()),
                FullName = dataReader["FullName"].ToString(),
                Email = dataReader["Email"].ToString(),
                Password = dataReader["Password"].ToString(),
                History = new List<HistoryModel>()
            };
        }
    }
}