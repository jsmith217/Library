using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace LibraryWeb.Repository
{
    public class ConnectionEstablisher
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["LibraryConnection"].ToString();
    }
}