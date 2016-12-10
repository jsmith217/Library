using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LibraryWeb.Models.History;

namespace LibraryWeb.Models.Readers
{
    public class ReaderModel
    {
        public int Id { get; set; }

        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Reader name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Reader email is required.")]
        [StringLength(100), MinLength(3)]
        public string Email { get; set; }
     
        [DisplayName("Password")]
        [StringLength(30)]
        public string Password { get; set; }
    }
}