using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clShared
{
    public static class SharedInfo
    {
        private static MySqlConnection _mySqlConnection;

        public static MySqlConnection getMySqlConnection()
        {
            if (_mySqlConnection == null)
            {
                _mySqlConnection =
                    new MySqlConnection("Server=77.66.48.115;Database=cluster_b;Uid=cluster_b;Pwd=password;CharSet=utf8;");
                _mySqlConnection.Open();
            }

            return _mySqlConnection;
        }

        public static void handleLogging(string data)
        {
            Console.WriteLine("ERROR: " + data);
        }
    }
}
