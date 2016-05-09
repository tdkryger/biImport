using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIImportProducts
{
    class BIImportProducts
    {
        static void Main(string[] args)
        {
            Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.Ok;
            Console.WriteLine("Importing products");
            importProducts();
        }

        static void importProducts()
        {
            if (!File.Exists(@"product_data.csv"))
            {
                clShared.SharedInfo.handleLogging("BIImportProducts", "product_data.csv not found");
                Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.FileNotFound;
                return;
            }
            bool firstLine = true;
            foreach (var line in File.ReadLines(@"product_data.csv"))
            {
                int id = 0;

                if (!firstLine)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 5)
                    {
                        if (int.TryParse(parts[0], out id))
                        {
                            using (MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                            {
                                //todo: handle duplicates this way?
                                try
                                {
                                    cmd.Connection = clShared.SharedInfo.getMySqlConnection();
                                    cmd.CommandText = "INSERT INTO `bi_Products` (`id`,`name`,`createDate`,`createdBy`) VALUES (@id,@name,@when,@who) "
                                                      + "ON DUPLICATE KEY UPDATE name=@name, createDate=@when, createdBy=@who;";
                                    cmd.Parameters.AddWithValue("id", id);
                                    cmd.Parameters.AddWithValue("name", parts[1]);
                                    cmd.Parameters.AddWithValue("when", DateTime.Now);
                                    cmd.Parameters.AddWithValue("who", "BIImportProducts");
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    clShared.SharedInfo.handleLogging("BIImportProducts", "insert data error: " + line + " (" + ex.Message + ")");
                                    Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.InsertError;
                                }
                            }
                        }
                        else
                        {
                            clShared.SharedInfo.handleLogging("BIImportProducts", "PK conversion error" + line);
                            Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.PKConversionError;
                        }
                    }
                    else
                    {
                        clShared.SharedInfo.handleLogging("BIImportProducts", "part length" + line);
                        Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.InvalidData;
                    }
                }
                firstLine = false;
            }
        }

    }
}
