using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace LibraryWeb.Models.Readers
{
    public class ReaderValidation
    {
        public static void Validate(ReaderModel reader)
        {
            try
            {
                var address = new MailAddress(reader.Email);
            }
            catch
            {
                throw new ArgumentException("Specified email is invalid.");
            }
        }
    }
}