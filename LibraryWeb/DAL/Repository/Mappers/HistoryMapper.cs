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
            string historyId = dataReader["HistoryId"].ToString();
            if (String.IsNullOrEmpty(historyId))
            {
                return new HistoryModel { Reader = this.MapReader(dataReader), Book = this.MapBook(dataReader) };
            }

            var returnDate = dataReader["DateReturned"].ToString();
            return new HistoryModel
            {
                Id = Int32.Parse(historyId),
                DateTaken = DateTime.Parse(dataReader["DateTaken"].ToString()),
                DateReturned = String.IsNullOrEmpty(returnDate) ? null : new Nullable<DateTime>(DateTime.Parse(returnDate)),
                Book = this.MapBook(dataReader),
                Reader = this.MapReader(dataReader)
            };
        }

        private BookModel MapBook(SqlDataReader reader)
        {
            var bookId = reader["BookId"].ToString();
            if (String.IsNullOrEmpty(bookId))
            {
                return null;
            }

            return new BookModel
            {
                Id = Int32.Parse(bookId),
                Title = reader["Title"].ToString().Trim(),
                AvailableQuantity = Int32.Parse(reader["Available"].ToString()),
                TotalQuantity = Int32.Parse(reader["Total"].ToString())
            };
        }

        private ReaderModel MapReader(SqlDataReader dataReader)
        {
            var readerId = dataReader["ReaderId"].ToString();
            if (String.IsNullOrEmpty(readerId))
            {
                return null;
            }

            return new ReaderModel
            {
                Id = Int32.Parse(readerId),
                FullName = dataReader["FullName"].ToString(),
                Email = dataReader["Email"].ToString(),
                Password = dataReader["Password"].ToString(),
                History = new List<HistoryModel>()
            };
        }
    }
}