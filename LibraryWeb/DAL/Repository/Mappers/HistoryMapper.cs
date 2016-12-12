using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Readers;
using LibraryWeb.Models.Authors;

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
                DateReturned = String.IsNullOrEmpty(returnDate) ? null : new Nullable<DateTime>(DateTime.Parse(returnDate))
            };
        }

        public BookModel MapBook(SqlDataReader reader)
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
                TotalQuantity = Int32.Parse(reader["Total"].ToString()),
                Authors = new List<AuthorModel>()
            };
        }

        public ReaderModel MapReader(SqlDataReader dataReader)
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

        public AuthorModel MapAuthor(SqlDataReader reader)
        {
            var authorId = reader["AuthorId"].ToString();
            if (String.IsNullOrEmpty(authorId))
            {
                return null;
            }

            return new AuthorModel
            {
                Id = Int32.Parse(authorId),
                FullName = reader["AuthorName"].ToString()
            };
        }
    }
}