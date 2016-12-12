using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Roles;

namespace LibraryWeb.Models.Readers
{
    public class ReaderModel
    {
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Full Name")]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [Required]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }
     
        [MaxLength(30)]
        public string Password { get; set; }

        [MaxLength(30)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match password.")]
        public string ConfirmPassword { get; set; }

        public RoleModel Role { get; set; }

        public List<HistoryModel> History { get; set; }
    }
}