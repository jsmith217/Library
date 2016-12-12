using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Readers;
using LibraryWeb.Repository;
using LibraryWeb.Models.History;
using System.Net.Mail;
using System.Xml;

namespace LibraryWeb.Service
{
    public class HistoryService : ICommonQueries<HistoryModel>
    {
        private HistoryRepository _historyRepo;
        private ReaderService _readerService;
        private BookService _bookService;
        //private MailNotificationSettings _notificationManager;

        public HistoryService()
        {
            this._historyRepo = new HistoryRepository();
            this._readerService = new ReaderService();
            this._bookService = new BookService();
            //this._notificationManager = new MailNotificationSettings();
        }

        public ReaderModel GetReaderHistory(int readerId)
        {
            // Handle not existing readers.
            var history = this._historyRepo.Select($"r.Id={readerId}");
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
            var history = this._historyRepo.Select($"b.Id={bookId}");
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
                AvailableQuantity = history.First().Book.AvailableQuantity,
                Authors = history.First().Book.Authors
            };
        }

        public void SendMail(HistoryModel history)
        {
           try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.To.Add(new MailAddress(history.Reader.Email));
                        message.Body = $"You have taken a book '{ history.Book.Title }' from the library.";

                        client.Send(message);
                    }
                }
            }
            catch (SmtpException ex)
            {
                throw new ArgumentException($"Notification was not sent due to internal errors.");
            }
        }

        public void TakeBook(HistoryModel history)
        {
            BookModel book = history.Book;
            ReaderModel reader = history.Reader;
            if (book == null || reader == null)
            {
                throw new ArgumentException("Reader can't take the book.");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (book.AvailableQuantity > 0)
                    {
                        book.AvailableQuantity--;
                        this._bookService.Update(book);
                        history.DateTaken = DateTime.Now;
                        this._historyRepo.Insert(history, connection);
                        // Send message;
                        this.SendMail(history);
                    }
                    else
                    {
                        throw new ArgumentException("Internal error. This books is not available");
                    }
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Connection error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void ReturnBook(HistoryModel history)
        {
            BookModel book = history.Book;
            ReaderModel reader = history.Reader;
            if (book == null || reader == null)
            {
                throw new ArgumentException("Reader can't return the book.");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (book.AvailableQuantity < book.TotalQuantity)
                    {
                        book.AvailableQuantity++;
                        this._bookService.Update(book);

                        history.DateReturned = DateTime.Now;
                        this._historyRepo.Update(history, connection);
                        // Send message;
                        this.SendMail(history);
                    }
                    else
                    {
                        throw new ArgumentException("Internal error. Total number of books can't be bigger than available.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Connection error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public HistoryModel GetById(int id)
        {
            return this._historyRepo.Select($"h.Id={id}").FirstOrDefault();
        }
    }
}