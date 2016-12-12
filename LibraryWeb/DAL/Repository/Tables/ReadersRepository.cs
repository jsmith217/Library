using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LibraryWeb.Models.Readers;
using LibraryWeb.Models.History;
using LibraryWeb.Models.Books;
using LibraryWeb.Repository.Mappers;

namespace LibraryWeb.Repository
{
    public class ReadersRepository : IRepository<ReaderModel>
    {
        private ReaderMapper _readerMapper;
        private ReadCommandBuilder _commandBuilder;
        private string _selectionString;

        public ReadersRepository()
        {
            this._readerMapper = new ReaderMapper();
            this._commandBuilder = new ReadCommandBuilder();
            this._selectionString = "SELECT r.*, roles.Id, roles.Name from Readers r INNER JOIN RoleData roles ON r.RoleId=roles.Id";
        }

        #region Write
        // Deprecate deletion.
        public void Delete(ReaderModel entity, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public void Insert(ReaderModel entity, SqlConnection connection)
        {
            string commandText = $"INSERT INTO Readers(Email, FullName, Password, RoleId) VALUES(@email, @fullName, @password, @roleId);";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@email", entity.Email);
                command.Parameters.AddWithValue("@fullName", entity.FullName);
                command.Parameters.AddWithValue("@password", entity.Password);
                command.Parameters.AddWithValue("@roleId", entity.Role.Id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong reader's insert query: {ex.Message}");
                }
            }
        }

        public void Update(ReaderModel entity, SqlConnection connection)
        {
            string commandText = $"UPDATE Readers SET FullName=@fullName, Email=@email, RoleId=@roleId Password=@password WHERE id=@id";
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@fullName", entity.FullName);
                command.Parameters.AddWithValue("@email", entity.Email);
                command.Parameters.AddWithValue("@password", entity.Password);
                command.Parameters.AddWithValue("@id", entity.Id);
                command.Parameters.AddWithValue("@roleId", entity.Role.Id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException($"Wrong reader's update query: {ex.Message}");
                }
            }
        }
        #endregion

        public List<ReaderModel> Select(params Pair[] conditions)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                return this.ExecuteCommand(this._commandBuilder.BuildSecureCommand(
                         this._selectionString,
                         connection,
                         "",
                         conditions));
            }
        }

        public List<ReaderModel> Select(params string[] conditions)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionEstablisher.ConnectionString))
            {
                return this.ExecuteCommand(this._commandBuilder.BuildNotSecureCommand(
                         this._selectionString,
                         connection,
                         "", conditions)).ToList();
            }
        }


        public List<ReaderModel> ExecuteCommand(SqlCommand command)
        {
            List<ReaderModel> readers = new List<ReaderModel>();
            try
            {
                command.Connection.Open();
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    readers.Add(this._readerMapper.Map(dataReader));
                }
                dataReader.Close();
            }
            catch (SqlException ex)
            {
                throw new ArgumentException($"Wrong select readers query: {ex.Message}");
            }
            finally
            {
                command.Connection.Close();
            }

            return readers;
        }
    }
}