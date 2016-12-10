using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Models.History
{
    public class HistoryValidation
    {
        public static void Validate(HistoryModel history)
        {
            if (history.DateReturned.CompareTo(history.DateTaken) < 0)
            {
                throw new ArgumentException("reader can't return book before taking one.");
            }
            if (history.Quantity < 0)
            {
                throw new ArgumentException("Reader can't take negative number of books from library.");
            }
        }
    }
}