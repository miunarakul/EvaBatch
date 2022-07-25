using System;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace EvaBatch.Web.Helper
{
    public class ConnectDB
    {
        OracleConnection conn;
        public void Connect(string dbKey)
        {
            conn = new OracleConnection();
            conn.ConnectionString = ConfigurationManager.AppSettings.Get(dbKey);
            conn.Open();
            Console.WriteLine("Connected to Oracle " + conn.DatabaseName);
        }

        public void Close()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}