using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
using System.Net.Mail;
using LibraryWeb.Models.Readers;
using LibraryWeb.Repository;
using LibraryWeb.Models.Books;

namespace LibraryWeb.Service
{
    public class ReaderService : ICommonQueries<ReaderModel>
    {
        private ReadersRepository _readersRepo;
        private BookService _bookService;
        private RoleService _roleService;

        public ReaderService()
        {
            this._readersRepo = new ReadersRepository();
            this._roleService = new RoleService();
            this._bookService = new BookService();
        }

        public List<ReaderModel> GetAllReaders()
        {
            return this._readersRepo.Select("");
        }

        public ReaderModel GetById(int id)
        {
            return this._readersRepo.Select(new Pair("r.Id", "=", $"{id}")).First();
        }

        public ReaderModel GetByDetails(ReaderModel reader)
        {
            string passwordCondition = $"Password='{reader.Password}'" ?? "Password IS NULL";
            var readers = this._readersRepo.Select(new Pair("Email", "=", $"'{reader.Email}'"));

            return readers.Count > 0 ? readers.First() : null; 
        }

        public ReaderModel GetByDetails(string email)
        {
            var readers = this._readersRepo.Select($"Email='{email}'");
            return readers.Count > 0 ? readers.First() : null;
        }

        public void CreateRegularReader(ReaderModel reader)
        {
            reader.Role = this._roleService.GetRegularRole();
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    this._readersRepo.Insert(reader, connection);
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Connection error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}