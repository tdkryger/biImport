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
            Console.WriteLine("Importing products");
            importProducts();
        }

        static void importProducts()
        {
            if (!File.Exists(@"product_data.csv"))
            {
                clShared.SharedInfo.handleLogging("product_data.csv not found");
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
