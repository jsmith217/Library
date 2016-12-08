using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Models.Books
{
    public class BookValidator
    {
        public static void Validate(BookModel book)
        {
            if (book.TotalQuantity <= 0)
            {
                throw new ArgumentException("Total quantity of the certain books can't be negative or 0.");
            }
            else if (book.AvailableQuantity < 0)
            {
                throw new ArgumentException("Total quantity of the available books can't be negative.");
            }
            else if (book.AvailableQuantity > book.TotalQuantity)
            {
                throw new ArgumentException("Quantity of the available books can't be bigger than total quantity.");
            }
            else if (book.Authors == null || book.Authors.Count == 0)
            {
                throw new ArgumentException("Book without the author cannot be added to the library.");
            }
        }
    }
}