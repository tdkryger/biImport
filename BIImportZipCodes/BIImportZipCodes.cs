using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIImportZipCodes
{
    class BIImportZipCodes
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Importing zips");
            importZipCodes();
        }

        static void importZipCodes()
        {
            if (!File.Exists(@"postnummer_data.csv"))
            {

                clShared.SharedInfo.handleLogging("postnummer_data.csv not found");
                return;
            }
            foreach (var line in File.ReadLines(@"postnummer_data.csv"))
            {
                int zipId = 0;
                string zip = string.Empty;
                string city = string.Empty;

                string[] parts = line.Split(';');
                if (parts.Length == 3)
                {
                    if (int.TryParse(parts[0], out zipId))
                    {
                        zip = parts[1];
                        city = parts[2];
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = clShared.SharedInfo.getMySqlConnection();
                            //todo: handle duplicates this way?
                            try
                            {
                                cmd.CommandText = "insert into bi_ZipCodes (id, zipCode, city, createDate, createdBy) VALUES (@id, @zipCode, @City, @when, @who) "
                                    + "ON DUPLICATE KEY UPDATE zipCode=@zipCode, city=@City, createDate=@when, createdBy=@who;";
                                cmd.Parameters.AddWithValue("id", zipId);
                                cmd.Parameters.AddWithValue("zipCode", zip);
                                cmd.Parameters.AddWithValue("City", city);
                                cmd.Parameters.AddWithValue("when", DateTime.Now);
                                cmd.Parameters.AddWithValue("who", "BIImportZipCodes");
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


        }
    }
}
