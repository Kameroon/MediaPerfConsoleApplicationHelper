using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppGeneratePDFFile.ExcelService
{
    public static class ExcelHelper
    {


        private static void SelectExcelFile(string connectionString)
        {
            // Connect EXCEL sheet with OLEDB using connection string
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter
                ("select * from [Sheet1$]", conn);
                DataSet excelDataSet = new DataSet();
                objDA.Fill(excelDataSet);
                //dataGridView1.DataSource = excelDataSet.Tables[0];
            }
        }

        private static void AddInExcelFile(string connectionString)
        {
            //In above code '[Sheet1$]' is the first sheet name with '$' as default selector,
            // with the help of data adaptor we can load records in dataset		

            //write data in EXCEL sheet (Insert data)
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"Insert into [Sheet1$] (month,mango,apple,orange) 
                                        VALUES ('DEC','40','60','80');";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //exception here
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private static void UpdateExcelFile(string connectionString)
        {
            //update data in EXCEL sheet (update data)
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE [Sheet1$] SET month = 'DEC' WHERE apple = 74;";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //exception here
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
    }
}
