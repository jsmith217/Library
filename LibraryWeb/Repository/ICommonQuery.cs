using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Repository
{
    public interface ICommonQuery<T> where T : class
    {
        T GetById(int id);

        List<T> GetAll();
    }
}