using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Readers;

namespace LibraryWeb.Models.History
{
    public class HistoryModel
    {
        public int Id { get; set; }

        public BookModel Book { get; set; }

        public ReaderModel Reader { get; set; }

        public short Quantity { get; set; }

        public DateTime DateTaken { get; set; }

        public DateTime DateReturned { get; set; }
    }
}