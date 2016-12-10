using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWeb.Service
{
    public interface ICommonQueries<T> where T : class
    {
        T GetById(int id);
    }
}