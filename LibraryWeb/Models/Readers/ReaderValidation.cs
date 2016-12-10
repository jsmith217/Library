using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Models.Readers
{
    public class ReaderValidation
    {
        public static void Validate(ReaderModel reader)
        {
            if (String.IsNullOrEmpty(reader.Email) || reader.Email.Length < 3)
            {
                throw new ArgumentException("Reader's email is not stated or invalid.");
            }
        }
    }
}