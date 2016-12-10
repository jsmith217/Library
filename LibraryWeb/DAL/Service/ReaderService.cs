using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryWeb.Models.Readers;
using LibraryWeb.Repository;

namespace LibraryWeb.Service
{
    public class ReaderService : ICommonQueries<ReaderModel>
    {
        private ReadersRepository _readersRepo;

        public ReaderService()
        {
            this._readersRepo = new ReadersRepository();
        }

        public List<ReaderModel> GetAllReaders()
        {
            return this._readersRepo.LightWeightSelect();
        }

        public ReaderModel GetById(int id)
        {
            return this._readersRepo.Select(new List<string> { $"r.Id={id}" }).First();
        }
    }
}