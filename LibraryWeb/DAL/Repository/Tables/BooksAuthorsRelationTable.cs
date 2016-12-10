using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Repository
{
    public class BooksAuthorsRelationTable
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        
        public int AuthorId { get; set; } 
    }
}