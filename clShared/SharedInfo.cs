using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clShared
{
    public static class SharedInfo
    {
        public enum ExitCodes
        {
            Ok=0,
            FileNotFound=1,
            InsertError=2,
            PKConversionError=3,
            InvalidData=4
        };

        private static MySqlConnection _mySqlConnection;

        public enum LogTypes { Debug, Info, Error };

        public static MySqlConnection getMySqlConnection()
        {
            if (_mySqlConnection == null)
            {
                //_mySqlConnection = new MySqlConnection("Server=77.66.48.115;Database=cluster_b;Uid=cluster_b;Pwd=password;CharSet=utf8;");
                _mySqlConnection = new MySqlConnection("Server=localhost;Database=cluster_b;Uid=thomas;Pwd=onakit8m;CharSet=utf8;");
                _mySqlConnection.Open();
            }

            return _mySqlConnection;
        }

        public static void handleLogging(string apppName, string data, LogTypes logType = LogTypes.Error)
        {
            string txt = string.Empty;
            switch(logType)
            {
                case LogTypes.Debug:
                    txt = txt + "Debug: ";
                    break;
                case LogTypes.Error:
                    txt = txt + "ERROR: ";
                    break;
                case LogTypes.Info:
                    txt = txt + "Info: ";
                    break;
            }
            txt = txt + data;
            Console.WriteLine(txt);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Log(txt, w);
            }
        }

        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
