using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Repository
{
    public class HistoryRelationTable 
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int ReaderId { get; set; }

        public DateTime DateTaken { get; set; }

        public DateTime DateReturned { get; set; }

        public int Quantity { get; set; }
    }
}