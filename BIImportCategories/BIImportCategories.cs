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
            importCategories();
        }

        static void importCategories()
        {
            if (!File.Exists(@"productcategory_data.csv"))
            {
                clShared.SharedInfo.handleLogging("productcategory_data.csv not found");
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
                                    clShared.SharedInfo.handleLogging("insert data error: " + line + " (" + ex.Message + ")");
                                }
                            }
                        }
                        else
                        {
                            clShared.SharedInfo.handleLogging("PK conversion error" + line);
                        }
                    }
                    else
                    {
                        clShared.SharedInfo.handleLogging("part length" + line);
                    }
                }
                firstLine = false;
            }
        }
    }
}
