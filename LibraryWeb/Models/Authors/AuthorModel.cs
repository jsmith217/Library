using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LibraryWeb.Models.Books;

namespace LibraryWeb.Models.Authors
{
    public class AuthorModel
    {
        public int Id { get; set; }

        [DisplayName("Full name")]
        [Required(ErrorMessage = "Full name of the author is a required information.")]
        [StringLength(100)]
        public string FullName { get; set; }

        public List<BookModel> Books { get; set; }
    }
}