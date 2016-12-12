using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Models.Books
{
    /// <summary>
    /// View model to get string of author names from view.
    /// </summary>
    public class BookViewModel
    {
        public BookModel Book { get; set; }

        public string AuthorString { get; set; }
    }
}