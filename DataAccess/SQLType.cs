using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class SQLType
    {
        private static readonly Dictionary<string, Type> dbTypeDic;
        private static readonly Dictionary<string, string> defaultSizeDic;

        static SQLType()
        {
            // - SQL to C# type dictionary -
            dbTypeDic = new Dictionary<string, Type>();

            // - Guid -
            dbTypeDic.Add("uniqueidentifier", typeof(Guid));

            // - Boolean -
            dbTypeDic.Add("bit", typeof(bool));

            // - Integer -
            dbTypeDic.Add("tinyint", typeof(byte));
            dbTypeDic.Add("smallint", typeof(short));
            dbTypeDic.Add("int", typeof(int));
            dbTypeDic.Add("bigint", typeof(long));

            // - Decimal -
            dbTypeDic.Add("real", typeof(float));
            dbTypeDic.Add("float", typeof(double));
            dbTypeDic.Add("decimal", typeof(decimal));
            dbTypeDic.Add("numeric", typeof(decimal));

            // - Text -
            dbTypeDic.Add("text", typeof(string));
            dbTypeDic.Add("char", typeof(string));
            dbTypeDic.Add("nchar", typeof(string));
            dbTypeDic.Add("varchar", typeof(string));
            dbTypeDic.Add("nvarchar", typeof(string));

            // - Date -
            dbTypeDic.Add("datetime", typeof(DateTime));
            dbTypeDic.Add("smalldatetime", typeof(DateTime));

            // - Binary -
            dbTypeDic.Add("image", typeof(byte[]));
            dbTypeDic.Add("binary", typeof(byte[]));
            dbTypeDic.Add("varbinary", typeof(byte[]));

            // - C# type to SQL default size -
            defaultSizeDic = new Dictionary<string, string>();
            defaultSizeDic.Add("decimal", "9,2");
            defaultSizeDic.Add("varchar", "255");
            defaultSizeDic.Add("nvarchar", "255");
        }

        static public Type GetType(string sqlType)
        {
            return dbTypeDic[sqlType];
        }

        #region - Get SQLType -
        static public string GetSQLType(Type type)
        {
            string sqlTypeFound = null;
            foreach (string sqlType in dbTypeDic.Keys)
            {
                if (dbTypeDic[sqlType] == type)
                {
                    sqlTypeFound = sqlType;
                    break;
                }
            }
            return sqlTypeFound;
        }
        #endregion

        #region - Get default size type -
        static public string GetDefaultSize(string sqlType)
        {
            return defaultSizeDic[sqlType];
        }
        #endregion

        #region - Set Decimal As Single - (Workaround)
        public static void SetDecimalAsSingle()
        {
            Type singleType = typeof(float);
            dbTypeDic["real"] = singleType;
            dbTypeDic["float"] = singleType;
            dbTypeDic["decimal"] = singleType;
            dbTypeDic["numeric"] = singleType;
        }
        #endregion
    }
}
