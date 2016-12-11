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
        private ReaderService _readerService;
        private BookService _bookService;

        public HistoryService()
        {
            this._historyRepo = new HistoryRepository();
            this._readerService = new ReaderService();
            this._bookService = new BookService();
        }

        public ReaderModel GetReaderHistory(int readerId)
        {
            // Handle not existing readers.
            var history = this._historyRepo.Select(new List<string> { $"r.Id={readerId}" });
            if (history.First().Book == null)
            {
                var reader = this._readerService.GetById(readerId);
                reader.History = null;
                return reader;
            }

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
            if (history.Count == 0 || history.First().Book == null)
            {
                var book = this._bookService.GetById(bookId);
                book.History = null;
                return book;
            }

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