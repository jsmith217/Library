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
            if (history.DateReturned.HasValue 
                && history.DateReturned.Value.CompareTo(history.DateTaken) < 0)
            {
                throw new ArgumentException("reader can't return book before taking one.");
            }
        }
    }
}