using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIImportOrderData
{
    class BIImportOrderData
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Importing orderData");
            importOrderData();
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
                                    cmd.Parameters.AddWithValue("who", "BIImportOrderData");
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
