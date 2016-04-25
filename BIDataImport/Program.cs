using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BIDataImport
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.WriteLine("Importing zips");
            importZipCodes();
            Console.WriteLine("Importing products");
            importProducts();
            Console.WriteLine("Importing customers");
            importCustomers();
            Console.WriteLine("Importing suppliers");
            importSuppliers();
            Console.WriteLine("Importing categorys");
            importCategories();
            Console.WriteLine("Importing orderData");
            importOrderData();

            Console.WriteLine("Done");
            Console.ReadLine();
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
                                cmd.Parameters.AddWithValue("who", "BiDataImport");
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
                            using (MySqlCommand cmd = new MySqlCommand())
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
                                    cmd.Parameters.AddWithValue("who", "BiDataImport");
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

        static void importCustomers()
        {
            if (!File.Exists(@"kunde_data.csv"))
            {
                clShared.SharedInfo.handleLogging("kunde_data.csv not found");
                return;
            }
            bool firstLine = false;
            foreach (var line in File.ReadLines(@"kunde_data.csv"))
            {
                int id = 0;

                if (!firstLine)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 3)
                    {
                        if (int.TryParse(parts[0], out id))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                //todo: handle duplicates this way?
                                try
                                {
                                    cmd.Connection = clShared.SharedInfo.getMySqlConnection();
                                    cmd.CommandText = "INSERT INTO `bi_Customers`(`id`,`name`,`createDate`,`createdBy`) VALUES (@id,@name,@when,@who) "
                                                      + "ON DUPLICATE KEY UPDATE name=@name, createDate=@when, createdBy=@who;";
                                    cmd.Parameters.AddWithValue("id", id);
                                    cmd.Parameters.AddWithValue("name", parts[1]);
                                    cmd.Parameters.AddWithValue("when", DateTime.Now);
                                    cmd.Parameters.AddWithValue("who", "BiDataImport");
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

        static void importSuppliers()
        {
            if (!File.Exists(@"suppliers_Data.csv"))
            {
                clShared.SharedInfo.handleLogging("suppliers_Data.csv not found");
                return;
            }
            bool firstLine = false;
            foreach (var line in File.ReadLines(@"suppliers_Data.csv"))
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
                                    cmd.CommandText = "INSERT INTO `bi_Suppliers` (`id`,`name`,`createDate`,`createdBy`) VALUES (@id,@name,@when,@who) "
                                                      + "ON DUPLICATE KEY UPDATE name=@name, createDate=@when, createdBy=@who;";
                                    cmd.Parameters.AddWithValue("id", id);
                                    cmd.Parameters.AddWithValue("name", parts[1]);
                                    cmd.Parameters.AddWithValue("when", DateTime.Now);
                                    cmd.Parameters.AddWithValue("who", "BiDataImport");
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
                                    cmd.Parameters.AddWithValue("who", "BiDataImport");
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

        static void importOrderData()
        {
            if (!File.Exists(@"order_data.csv"))
            {
                clShared.SharedInfo.handleLogging("order_data.csv not found");
                return;
            }
            bool firstLine = true;
            foreach (var line in File.ReadLines(@"order_data.csv"))
            {
                int orderId = 0;
                int zipId = 0;
                int categoryId = 0;
                int customerId = 0;
                int productId = 0;
                int supplierId = 0;
                int count = 0;
                decimal sum = 0;

                if (!firstLine)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 8)
                    {
                        bool conversionError = true;
                        if (int.TryParse(parts[0], out orderId))
                            if (decimal.TryParse(parts[7], out sum))
                                if (int.TryParse(parts[6], out count))
                                    if (int.TryParse(parts[1], out customerId))
                                        if (int.TryParse(parts[2], out zipId))
                                            if (int.TryParse(parts[3], out productId))
                                                if (int.TryParse(parts[4], out categoryId))
                                                    if (int.TryParse(parts[5], out supplierId))
                                                        conversionError = false;

                        if (!conversionError)
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                //todo: handle duplicates this way?
                                try
                                {
                                    cmd.Connection = clShared.SharedInfo.getMySqlConnection();
                                    cmd.CommandText = "INSERT INTO `bi_TotalSales` (`orderId`,`zipId`,`categoryId`,`customerId`,`productId`,`supplierId`,`sales`,`units`,`createDate`,`createdBy`) "
                                                      +
                                                      " VALUES (@orderId,@zipId,@categoryId,@customerId,@productId,@supplierId,@sales,@units,@when,@who) "
                                                      +
                                                      " ON DUPLICATE KEY UPDATE sales=@sales, units=@units, createDate=@when, createdBy=@who;";
                                    cmd.Parameters.AddWithValue("orderId", orderId);
                                    cmd.Parameters.AddWithValue("zipId", zipId);
                                    cmd.Parameters.AddWithValue("categoryId", categoryId);
                                    cmd.Parameters.AddWithValue("customerId", customerId);
                                    cmd.Parameters.AddWithValue("productId", productId);
                                    cmd.Parameters.AddWithValue("supplierId", supplierId);
                                    cmd.Parameters.AddWithValue("sales", sum);
                                    cmd.Parameters.AddWithValue("units", count);
                                    cmd.Parameters.AddWithValue("when", DateTime.Now);
                                    cmd.Parameters.AddWithValue("who", "BiDataImport");
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
                            clShared.SharedInfo.handleLogging("Conversion error" + line);
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
