using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryWeb.Models.Books;

namespace LibraryWeb.Models.Readers
{
    public class ReaderBooksViewModel
    {
        public BookModel Book { get; set; }

        public int Quantity { get; set; }

        public DateTime DateTaken { get; set; }

        public ReaderBooksViewModel(BookModel book, int quantity, DateTime date)
        {
            this.Book = book;
            this.Quantity = quantity;
            this.DateTaken = date;
        }
    }
}