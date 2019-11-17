using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleAppITextSharpPDF.DataAccess
{
    public class MyDataAccess
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Author> GetAuthors()
        {
            var date = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
            var authorList = new List<Author>();

            authorList.Add(new Author("Mahesh Chand",
                35,
                "A Prorammer's Guide to ADO.NET",
                true,
                date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30,
                "Building Creative Systems",
                false,
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30,
                "Building Creative Systems",
                false,
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "Graphics Programming with GDI+",
               true,
               date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30,
                "Building Creative Systems",
                false,
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "A Prorammer's Guide to ADO.NET",
                true,
                date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "Graphics Programming with GDI+",
               true,
               date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30,
                "Building Creative Systems",
                false,
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));


            return authorList;
        }


        public static string GetConnectionString()
        {
            string connetionString = "Data Source=DESKTOP-6DSO6AT\\SQLEXPRESS; Database=JunkEFCodeFrist1; Trusted_Connection=True";

            if (ConfigurationManager.ConnectionStrings["MyDbConnectionString"] == null)
            {
                return connetionString;
            }
            else
            {
                return ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
            }
        }

        /// <summary>
        /// -- Create SQL Connection --
        /// </summary>
        /// <returns></returns>
        private static SqlConnection CreateConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        /// <summary>
        /// OK
        /// </summary>
        public static object GetSingleDataObjectSyncOK()
        {
            dynamic record = null;
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var param = new DynamicParameters();
                param.Add("@personTypeId", 2);
                param.Add("@CompagnyId", 3);

                record = connection.QueryFirstOrDefault("GetAllXMLDataObjects", null,
                                                     commandType: CommandType.StoredProcedure);
            }
            
            return record;
        }

        /// <summary>
        /// - OK -  GetAllXMLDataObjects
        /// </summary>
        /// <returns></returns>
        public static object GetDataSync()
        {
            string query = @"SELECT * FROM[JunkEFCodeFrist1].[dbo].[People]";
            query += @"SELECT * FROM[JunkEFCodeFrist1].[dbo].[Compagnies]";

            dynamic record = null;
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                using (var data = conn.QueryMultiple("GetAllXMLDataObjects", null))
                {
                    record = data.Read<dynamic>();
                    var values = data.Read<dynamic>();
                }
            }

            return record;
        }


        #region --  --
        public static void GetAllData()
        {

            string proc = "GetAllDataObjects";
            try
            {

                //var res = ExecuteProc(CreateConnection(), proc, null);

                SelectQuery(CreateConnection(), proc);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        #endregion

        #region -- DAPPER GENERIC --
        private static bool ExecuteProc(SqlConnection sqlConnection,
           string sql, List<SqlParameter> paramList = null)
        {
            try
            {
                using (SqlConnection conn = sqlConnection)
                {
                    DynamicParameters dp = new DynamicParameters();

                    if (paramList != null)
                        foreach (SqlParameter sp in paramList)
                            dp.Add(sp.ParameterName, sp.SqlValue, sp.DbType);

                    conn.Open();

                    return conn.Execute(sql, dp, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception e)
            {
                //do logging
                return false;
            }
        }

        private static void SelectQuery(SqlConnection sqlConnection,
          string sql, List<SqlParameter> paramList = null)
        {
            try
            {
                using (SqlConnection conn = sqlConnection)
                {
                    DynamicParameters dp = new DynamicParameters();

                    if (paramList != null)
                        foreach (SqlParameter sp in paramList)
                            dp.Add(sp.ParameterName, sp.SqlValue, sp.DbType);
                    
                    conn.Open();

                    var res = conn.Query(sql, paramList, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                //do logging
                //return false;
            }
        }
        #endregion
    }
}
