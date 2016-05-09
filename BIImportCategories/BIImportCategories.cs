using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIImportCategories
{
    class BIImportCategories
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Importing categorys");
            Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.Ok;
            importCategories();
        }

        static void importCategories()
        {
            if (!File.Exists(@"productcategory_data.csv"))
            {
                clShared.SharedInfo.handleLogging("BIImportCategories", "productcategory_data.csv not found");
                Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.FileNotFound;
                return;
            }
            bool firstLine = false;
            foreach (var line in File.ReadLines(@"productcategory_data.csv"))
            {
                int id = 0;

                if (!firstLine)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        if (int.TryParse(parts[0], out id))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                //todo: handle duplicates this way?
                                try
                                {
                                    cmd.Connection = clShared.SharedInfo.getMySqlConnection();
                                    cmd.CommandText = "INSERT INTO `bi_ProductCategories` (`id`,`productCategory`,`createDate`,`createdBy`) VALUES (@id,@productCategory,@when,@who) "
                                                      + "ON DUPLICATE KEY UPDATE productCategory=@productCategory, createDate=@when, createdBy=@who;";
                                    cmd.Parameters.AddWithValue("id", id);
                                    cmd.Parameters.AddWithValue("productCategory", parts[1]);
                                    cmd.Parameters.AddWithValue("when", DateTime.Now);
                                    cmd.Parameters.AddWithValue("who", "BIImportCategories");
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    clShared.SharedInfo.handleLogging("BIImportCategories", "insert data error: " + line + " (" + ex.Message + ")");
                                    Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.InsertError;
                                }
                            }
                        }
                        else
                        {
                            clShared.SharedInfo.handleLogging("BIImportCategories", "PK conversion error" + line);
                            Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.PKConversionError;
                        }
                    }
                    else
                    {
                        clShared.SharedInfo.handleLogging("BIImportCategories", "part length" + line);
                        Environment.ExitCode = (int)clShared.SharedInfo.ExitCodes.InvalidData;
                    }
                }
                firstLine = false;
            }
        }
    }
}
