using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppGeneratePDFFile
{
    public class Helper
    {

        //ConnectionStringSettings mySQLConSettings = ConfigurationManager.ConnectionStrings["MyDBConnection"];

        public static string GetConnectionString()
        {
            string con = ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
            return ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
        }
    }

    #region MyRegion
   
    #endregion
}
