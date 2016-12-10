using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LibraryWeb.Models.Authors;
using LibraryWeb.Models.History;

namespace LibraryWeb.Models.Books
{
    public class BookModel
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(250)]
        public string Title { get; set; }

        [DisplayName("Total quantity")]
        [Required(ErrorMessage = "Total quantity of the books is a required information.")]
        public int TotalQuantity { get; set; }

        [DisplayName("Available quantity")]
        [Required(ErrorMessage = "Quantity of the available books is a required information.")]
        public int AvailableQuantity { get; set; }

        [DisplayName("Status")]
        public BookStatus Status
        {
            get
            {
                if (this.AvailableQuantity != 0)
                {
                    return BookStatus.YES;
                }

                return BookStatus.NO;
            }
        }

        public List<AuthorModel> Authors { get; set; }

        public List<HistoryModel> History { get; set; }
    }
}