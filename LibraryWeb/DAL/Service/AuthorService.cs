using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Repository;
using LibraryWeb.Models.Authors;

namespace LibraryWeb.Service
{
    public class AuthorService : ICommonQueries<AuthorModel>
    {
        private AuthorsRepository _authorRepo;

        public AuthorService()
        {
            this._authorRepo = new AuthorsRepository();
        }

        public void CreateAuthor(AuthorModel author)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                try
                {
                    connection.Open();
                    this._authorRepo.Insert(author, connection);
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Internal server error. Connection not established.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<AuthorModel> GetAllAuthors()
        {
            return this._authorRepo.Select();
        }

        public AuthorModel GetById(int id)
        {
            return this._authorRepo.Select($"Id={id}").FirstOrDefault();
        }

        public AuthorModel GetAuthorByName(string name)
        {
            return this._authorRepo.Select($"FullName='{name}'").FirstOrDefault();
        }

        public void Edit(AuthorModel author)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
               try
                {
                    connection.Open();
                    this._authorRepo.Update(author, connection);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Internal server error. Connection not established.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}