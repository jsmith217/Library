using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Readers;
using LibraryWeb.Repository;

namespace LibraryWeb.Service
{
    public class HistoryService
    {
        private HistoryRepository _historyRepo;

        public HistoryService()
        {
            this._historyRepo = new HistoryRepository();
        }

        public ReaderModel GetReaderHistory(int readerId)
        {
            var history = this._historyRepo.Select(new List<string> { $"r.Id={readerId}" });
            return new ReaderModel
            {
                Id = history.First().Reader.Id,
                FullName = history.First().Reader.FullName,
                Email = history.First().Reader.Email,
                Password = history.First().Reader.Password,
                History = history
            };
        }

        public BookModel GetBookHistory(int bookId)
        {
            var history = this._historyRepo.Select(new List<string> { $"b.Id={bookId}" });
            return new BookModel
            {
                History = history,
                Id = history.First().Id,
                Title = history.First().Book.Title,
                TotalQuantity = history.First().Book.TotalQuantity,
                AvailableQuantity = history.First().Book.AvailableQuantity
            };
        }
    }
}