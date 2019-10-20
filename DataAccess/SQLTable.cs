using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    /// <summary>
    /// SQL Server table.
    /// </summary>
    public class SQLTable : IDisposable
    {
        #region - Constants -
        private const int DEFAULT_CMD_TIMEOUT = 1200; //-> 1200sec = 20 minutes
        private const string DEFAULT_SCHEMA = "dbo";
        #endregion

        #region - Attributes (Protected) -
        protected SQLDataBase sqlDB;
        protected string schema, tableName;
        protected DataTable dataTable;
        #endregion

        /// <summary>
        /// Create instance of SQLTable and read definition from SQL Server.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlDatabase"></param>
        public SQLTable(string tableName, SQLDataBase sqlDB) : this(sqlDB, DEFAULT_SCHEMA, tableName) { }

        /// <summary>
        /// Create instance of SQLTable and read definition from SQL Server.
        /// </summary>
        /// <param name="tableName">Name of the table on SQL Server.</param>
        /// <param name="tableName">Name of the schema.</param>
        /// <param name="sqlDatabase">Destination SQLDatabase.</param>
        public SQLTable(SQLDataBase sqlDB, string schema, string tableName, bool withKeyInfo = true)
        {
            // - Save arguments -
            this.sqlDB = sqlDB;
            this.schema = schema;
            this.tableName = tableName;

            // - Display warning as this new method could produce bugs -
            System.Diagnostics.Debug.Print("Warning : GetEmptyResult method used !");

            // - Get table definition -
            this.dataTable = sqlDB.GetEmptyResult("SELECT * FROM {0}.{1}", schema, tableName);

            //// - Read definition from SQL Server -
            //this.ReadTableSchema();
        }

        /// <summary>
        /// Create instance of SQLTable and use given System.Data.DataTable.
        /// </summary>
        /// <param name="tableName">Name of the table on SQL Server.</param>
        /// <param name="dataTable">DataTable.</param>
        /// <param name="sqlDB">Destination SQLDatabase.</param>
        public SQLTable(string tableName, DataTable dataTable, SQLDataBase sqlDatabase)
        {
            this.schema = DEFAULT_SCHEMA;
            this.tableName = tableName;
            this.dataTable = dataTable;
            this.sqlDB = sqlDatabase;
        }

        #region - INSERT INCOMPLET !!
        //private void InsertTemp()
        //{
        //    SqlConnection sqlConn;
        //    SqlCommand bcCMD, bapCMD;
        //    String cmdText;
        //    // - Open SQL Connection - (Will be closed only at the application end)
        //    sqlConn = sqlDatabase.GetNewOpenedConnection();
        //    // - Prepare BackupCustomer statement -
        //    cmdText = "INSERT INTO TD_BACKUP_CUSTOMER (CUSTOMER_ID, SALES_ORG, TERRITORY, SELECTED) VALUES (@CUSTOMER_ID, @SALES_ORG, @TERRITORY, @SELECTED)";
        //    bcCMD = new SqlCommand(cmdText, sqlConn);
        //    bcCMD.Parameters.Add("@CUSTOMER_ID", SqlDbType.VarChar, 10);
        //    bcCMD.Parameters.Add("@SALES_ORG", SqlDbType.VarChar, 4);
        //    bcCMD.Parameters.Add("@TERRITORY", SqlDbType.VarChar, 4);
        //    // - Prepare BackupActionPlan statement -
        //    cmdText = "INSERT INTO TD_BACKUP_ACTION_PLAN (CUSTOMER_ID, SALES_ORG, GOAL, CONTACT, HOW, WHO, [WHEN]) VALUES (@CUSTOMER_ID, @SALES_ORG, @GOAL, @HOW, @WHO, @[WHEN])";
        //    bcCMD = new SqlCommand(cmdText, sqlConn);
        //}
        #endregion

        #region - Read TableSchema from SQL Server -
        private void ReadTableSchema()
        {
            // - Declare temp object -
            object objTmp;
            // - Building SQL request (Table Schema) -
            string sqlCmdStr = "SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema ='{0}' AND table_name='{1}'";
            sqlCmdStr = string.Format(sqlCmdStr, schema, tableName);
            // Open connection:
            SqlConnection sqlConn = sqlDB.GetNewConnection();
            sqlConn.Open();
            // New sql command:
            SqlCommand sqlCmd = GetNewSqlCommand(sqlCmdStr, sqlConn);
            // Init:
            dataTable = new DataTable(tableName);
            // Go:
            SqlDataReader sRdr = sqlCmd.ExecuteReader();
            while (sRdr.Read())
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = (string)sRdr["COLUMN_NAME"];
                dataColumn.DataType = TryGetType(sRdr["DATA_TYPE"]);
                dataColumn.AllowDBNull = TryGetBoolYesNo(sRdr["IS_NULLABLE"]);
                //dataColumn.MaxLength = DBNull.Value.Equals(sRdr["CHARACTER_MAXIMUM_LENGTH"]) ? -1 : (Int32)sRdr["CHARACTER_MAXIMUM_LENGTH"];
                try
                {
                    dataColumn.MaxLength = (Int32)sRdr["CHARACTER_MAXIMUM_LENGTH"];
                }
                catch (Exception) { }
                // - Default value -
                objTmp = sRdr["COLUMN_DEFAULT"];
                if (objTmp != null && objTmp != DBNull.Value)
                {
                    string defaultValue = objTmp.ToString();
                    // - Remove special char -  Ex: ('0900')
                    defaultValue = defaultValue.Replace("(", string.Empty);
                    defaultValue = defaultValue.Replace(")", string.Empty);
                    if (defaultValue.Contains("'"))
                        defaultValue = defaultValue.Replace("'", string.Empty);
                    // - Set default value -
                    try
                    {
                        dataColumn.DefaultValue = defaultValue;
                    }
                    catch (Exception) { Console.WriteLine(string.Format("SQLTable - Unable to set column default value : {0}", objTmp)); }
                }
                // - Add row to table -
                dataTable.Columns.Add(dataColumn);
            }
            // Close:
            FreeMemory(sqlCmd, sRdr);
            // Reading Primary Key Column name:
            try
            {
                sqlCmdStr = "SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey')=1 AND table_name='{0}' ORDER BY ORDINAL_POSITION";
                sqlCmdStr = string.Format(sqlCmdStr, tableName);
                sqlCmd = GetNewSqlCommand(sqlCmdStr, sqlCmd.Connection);
                // Go:
                List<DataColumn> pkList = new List<DataColumn>();
                sRdr = sqlCmd.ExecuteReader();
                while (sRdr.Read())
                {
                    string columnName = (string)sRdr["column_name"];
                    pkList.Add(dataTable.Columns[columnName]);
                }
                dataTable.PrimaryKey = pkList.ToArray();
                // - Free ressources -
                pkList.Clear();
                FreeMemory(sqlCmd, sRdr);
            }
            catch (Exception e)
            {
                Console.WriteLine("Primary Key: " + e.Message);
            }
            // Reading IDENTITY Column name:
            try
            {
                sqlCmdStr = "SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME),COLUMN_NAME,'ISIdentity')=1 AND table_name='{0}'";
                sqlCmdStr = string.Format(sqlCmdStr, tableName);
                sqlCmd = GetNewSqlCommand(sqlCmdStr, sqlCmd.Connection);
                // Go:
                string identity = sqlCmd.ExecuteScalar() as string;
                if (identity != null)
                {
                    dataTable.Columns[identity].AutoIncrement = true;
                    dataTable.Columns[identity].AutoIncrementSeed = 1;
                }
                // Close:
                FreeMemory(sqlCmd, sRdr);
            }
            catch (Exception e)
            {
                Console.WriteLine("IDENTITY Column: " + e.Message);
            }
            // - Free memory -
            FreeMemory(sqlConn);
        }
        #endregion

        #region - Properties -
        public SQLDataBase SQLDataBaseLocal
        {
            get { return sqlDB; }
        }
        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }
        public DataTable Table
        {
            get { return dataTable; }
            set { dataTable = value; }
        }
        /// <summary>
        /// Table name in SQL Server.
        /// </summary>
        public string TableName
        {
            get { return tableName; }
        }
        #endregion

        #region - Disable PrimaryKey -
        public void DisablePrimaryKey()
        {
            dataTable.PrimaryKey = new DataColumn[0];
        }
        #endregion

        #region - Read data -
        /// <summary>
        /// Select all data from the table
        /// </summary>
        /// <returns>System.Data.DataTable that contains all data</returns>
        public DataTable SelectAll()
        {
            return ReadAll();
        }

        public DataTable ReadAll()
        {
            // Reading Table Schema:
            string sqlCmdStr = string.Format("SELECT * FROM {0}.{1}", schema, tableName);
            // Open connection:
            SqlConnection sqlConn = sqlDB.GetNewConnection();
            sqlConn.Open();
            // New sql command:
            SqlCommand sqlCmd = GetNewSqlCommand(sqlCmdStr, sqlConn);
            // Go:
            SqlDataReader sRdr = sqlCmd.ExecuteReader();
            Fill(sRdr);
            // Close:
            FreeMemory(sqlCmd, sRdr, sqlConn);
            return dataTable;
        }

        public void Fill(SqlDataReader sRdr)
        {
            while (sRdr.Read())
            {
                try
                {
                    DataRow dRow = dataTable.NewRow();
                    for (int i = 0; i < sRdr.FieldCount; i++)
                        dRow[i] = sRdr[i];
                    dataTable.Rows.Add(dRow);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion

        #region - Truncate (Erase data) -
        /// <summary>
        /// Truncate table (All data erased, Auto-Increment reset, etc.)
        /// </summary>
        public void Truncate()
        {
            Truncate(sqlDB, schema, tableName);
        }
        public static void Truncate(SQLDataBase sqlDB, string tableName)
        {
            Truncate(sqlDB, DEFAULT_SCHEMA, tableName);
        }
        public static void Truncate(SQLDataBase sqlDB, string schema, string tableName)
        {
            sqlDB.ExecuteNonQuery("TRUNCATE TABLE {0}.{1}", schema, tableName);
        }
        //public Boolean EraseTable()
        //{
        //    String sqlCmdStr; SqlConnection sqlConn; SqlCommand sqlCmd;
        //    try
        //    {
        //        sqlCmdStr = "TRUNCATE TABLE {0}";
        //        sqlCmdStr = String.Format(sqlCmdStr, tableName);
        //        // Open connection:
        //        sqlConn = sqlDB.GetNewConnection();
        //        sqlConn.Open();
        //        // New sql command:
        //        sqlCmd = GetNewSqlCommand(sqlCmdStr, sqlConn);
        //        sqlCmd.ExecuteNonQuery();
        //        // Close:
        //        FreeMemory(sqlCmd, sqlConn);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return false;
        //    }
        //}
        #endregion

        #region - Import (From DataTable) -
        /// <summary>
        /// Import all rows of a DataTable
        /// </summary>
        /// <param name="srcTable"></param>
        public void Import(DataTable srcTable)
        {
            for (int i = 0; i < srcTable.Rows.Count; i++)
            {
                dataTable.ImportRow(srcTable.Rows[i]);
            }
        }
        #endregion

        #region - Add row -
        /// <summary>
        /// Add row from values array.
        /// </summary>
        /// <param name="values">Array of values to insert into a new row.</param>
        /// <returns></returns>
        public DataRow AddRow(params object[] values)
        {
            // - Add row to the DataTable -
            return dataTable.Rows.Add(values);
        }
        #endregion

        #region - WriteToServer -
        public bool WriteToServer()
        {
            return WriteToServer(tableName, dataTable);
        }
        public bool WriteToServer(DataTable dataTableArg)
        {
            return WriteToServer(dataTableArg.TableName, dataTableArg);
        }

        public bool WriteToServer(string tableNameArg, DataTable dataTableArg)
        {
            SqlConnection sqlConn; SqlBulkCopy sqlBulkCopy;
            try
            {
                // Open connection:
                sqlConn = sqlDB.GetNewOpenedConnection();

                // - Init -
                sqlBulkCopy = new SqlBulkCopy(sqlConn);
                sqlBulkCopy.BulkCopyTimeout = 15 * 60;  //-> 15 min !
                sqlBulkCopy.DestinationTableName = tableNameArg;
                sqlBulkCopy.BatchSize = dataTableArg.Rows.Count;

                // - Run -
                sqlBulkCopy.WriteToServer(dataTableArg);

                // - Free memory -
                sqlBulkCopy.Close();
                sqlBulkCopy = null;
                FreeMemory(sqlConn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Bulk insert : {0}", e.Message);
                return false;
            }
        }
        #endregion

        #region - WriteToServer (TableLock) -
        /// <summary>
        /// Copy all rows from DataTable to SQL Server table. Table is locked during the process.
        /// </summary>
        /// <returns>Return true in case of success in false in case of failure.</returns>
        public bool WriteToServerTL(bool setUpdateDate = false)
        {
            return WriteToServerTL(tableName, dataTable, setUpdateDate);
        }

        private bool WriteToServerTL(string tableNameArg, DataTable dataTableArg, bool setUpdateDate = false)
        {
            // - Declare SqlBulkCopy -
            SqlBulkCopy sqlBulkCopy = null;

            try
            {
                // - Build SqlBulkCopy -
                sqlBulkCopy = new SqlBulkCopy(sqlDB.ConnectionString, SqlBulkCopyOptions.TableLock);
                sqlBulkCopy.BulkCopyTimeout = 15 * 60;  //-> 15 min !
                sqlBulkCopy.DestinationTableName = string.Format("{0}.{1}", schema, tableNameArg);
                sqlBulkCopy.BatchSize = dataTableArg.Rows.Count;

                // - Run -
                sqlBulkCopy.WriteToServer(dataTableArg);

                // - Set UpdateDate -
                if (setUpdateDate == true)
                {
                    SetUpdateDate();
                }

                // - Return true in case of success -
                return true;
            }
            catch (Exception e)
            {
                // - Display error message -
                Console.WriteLine("BulkCopy error : {0}" + e.Message);

                // - Return false in case of error -
                return false;
            }
            finally
            {
                // - Close SqlBulkCopy -
                sqlBulkCopy?.Close();
            }
        }
        #endregion

        #region - Select (Formated) -
        public DataRow[] Select(string filterExpression, params object[] argList)
        {
            return dataTable.Select(string.Format(filterExpression, argList));
        }
        #endregion

        #region - Select List -
        /// <summary>
        /// Return a list instead of a row array (Memory)
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="query"></param>
        /// <param name="argList"></param>
        /// <returns></returns>
        public List<T> SelectList<T>(string columnName, string query, params object[] argList)
        {
            // - Initialize list -
            List<T> list = new List<T>();

            // - Select -
            DataRow[] results = Select(query, argList);

            // - Build list -
            for (int i = 0; i < results.Length; i++)
                list.Add((T)results[i][columnName]);
            return list;
        }
        #endregion

        #region - Find (Memory) -
        /// <summary>
        /// Return the row corresponding to the given key list (Memory)
        /// </summary>
        /// <param name="keyList"></param>
        /// <returns></returns>
        public DataRow Find(params object[] keyList)
        {
            return dataTable.Rows.Find(keyList);
        }
        #endregion

        #region - Contains (Memory) -
        /// <summary>
        /// Check if there is a row corresponding to the given key list (Memory PK)
        /// </summary>
        /// <param name="keyList"></param>
        /// <returns></returns>
        public bool Contains(params object[] keyList)
        {
            return dataTable.Rows.Contains(keyList);
        }
        #endregion

        #region - Contains (SQL) -
        protected bool Contains(int sqlColID, int sapColID, DataRow sapRow)  // String Primary Key only !
        {
            bool contains = true; int count; string sqlCmdStr, pkColumn, pkValue; SqlConnection sqlConn; SqlCommand sqlCmd;
            try
            {
                pkColumn = dataTable.Columns[sqlColID].ColumnName;
                pkValue = (string)sapRow[sapColID];
                // Reading Table Schema:
                sqlCmdStr = "SELECT COUNT(*) FROM {0} WHERE {1} = '{2}'";
                sqlCmdStr = string.Format(sqlCmdStr, tableName, pkColumn, pkValue);
                // Open connection:
                sqlConn = sqlDB.GetNewConnection();
                sqlConn.Open();
                // New sql command:
                sqlCmd = GetNewSqlCommand(sqlCmdStr, sqlConn);
                count = (int)sqlCmd.ExecuteScalar();
                if (count == 0)
                    contains = false;
                else
                    contains = true;
                // Close:
                FreeMemory(sqlCmd, sqlConn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return contains;
        }
        protected bool ContainsObject(int sqlColID, int sapColID, DataRow sapRow)
        {
            bool contains = true; int count; string sqlCmdStr, pkColumn; SqlConnection sqlConn; SqlCommand sqlCmd;
            object pkValue;
            try
            {
                pkColumn = dataTable.Columns[sqlColID].ColumnName;
                pkValue = sapRow[sapColID];
                // Reading Table Schema:
                sqlCmdStr = "SELECT COUNT(*) FROM {0} WHERE {1} = {2}";
                sqlCmdStr = string.Format(sqlCmdStr, tableName, pkColumn, pkValue);
                // Open connection:
                sqlConn = sqlDB.GetNewConnection();
                sqlConn.Open();
                // New sql command:
                sqlCmd = GetNewSqlCommand(sqlCmdStr, sqlConn);
                count = (int)sqlCmd.ExecuteScalar();
                if (count == 0)
                    contains = false;
                // Close:
                FreeMemory(sqlCmd, sqlConn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return contains;
        }
        #endregion

        #region - Exists (SQL) -
        /// <summary>
        /// Check if a row match the WHERE criteria. (SQL Query / Include already the WHERE keyword)
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Exists(string whereClause, params object[] args)
        {
            return Exists(string.Format(whereClause, args));
        }

        /// <summary>
        /// Check if a row match the WHERE criteria. (SQL Query / Include already the WHERE keyword)
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public bool Exists(string whereClause)
        {
            return Exists(sqlDB, tableName, whereClause);
        }

        /// <summary>
        /// Check if a row match the WHERE criteria. (SQL Query / Include already the WHERE keyword)
        /// </summary>
        /// <param name="anyDB"></param>
        /// <param name="tableName"></param>
        /// <param name="whereClause"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool Exists(SQLDataBase anyDB, string tableName, string whereClause, params object[] args)
        {
            return Exists(anyDB, tableName, string.Format(whereClause, args));
        }

        /// <summary>
        /// Check if a row match the WHERE criteria. (SQL Query / Include already the WHERE keyword)
        /// </summary>
        /// <param name="anyDB"></param>
        /// <param name="anyTableName"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public static bool Exists(SQLDataBase anyDB, string tableName, string whereClause)
        {
            // - Return true if at least 1 line meat the where criteria -
            return (int)anyDB.ExecuteScalar(string.Format("SELECT CASE WHEN EXISTS (SELECT * FROM {0} WHERE {1}) THEN 1 ELSE 0 END", tableName, whereClause)) == 1;
        }
        #endregion

        #region - SQL Function -
        static public SqlCommand GetNewSqlCommand(string command, SqlConnection sqlConn)
        {
            SqlCommand cmd = new SqlCommand(command, sqlConn);
            cmd.CommandTimeout = DEFAULT_CMD_TIMEOUT;
            return cmd;
        }
        #endregion

        #region - Dispose -
        /// <summary>
        /// Clear all data and call "DataTable.Dispose()" method (E2MKI)
        /// </summary>
        public void Dispose()
        {
            FreeMemory(dataTable);
            // - Clear reference -
            dataTable = null;
            sqlDB = null;
            tableName = null;
        }
        #endregion

        #region - Static method Get DataTable -
        public static DataTable GetDataTable(string tableName, SQLDataBase sqlDB)
        {
            SQLTable sqlTable = new SQLTable(tableName, sqlDB);
            return sqlTable.ReadAll();
        }
        #endregion

        #region - SetUpdateDate -
        private void SetUpdateDate()
        {
            SetUpdateDate(sqlDB.Name, tableName, sqlDB);
        }
        static public void SetUpdateDate(string databaseNameToUpdate, string tableNameUpdated, SQLDataBase anySqlDB)
        {
            try
            {
                // - Init -
                bool rowExist = false;

                // - Init SQL Query -
                string statement = "SELECT CASE WHEN EXISTS (SELECT * FROM [E2MKI-MasterData].dbo.TD_UpdateDate WHERE DatabaseName='{0}' AND TableName='{1}') THEN 1 ELSE 0 END";
                statement = string.Format(statement, databaseNameToUpdate, tableNameUpdated);

                // - Run SQL Query -
                if ((int)anySqlDB.ExecuteScalar(statement) == 1)
                    rowExist = true;

                // - If row exists Update else Insert -
                if (rowExist == true)
                    statement = "UPDATE [E2MKI-MasterData].dbo.TD_UpdateDate SET UpdateDate=GETDATE() WHERE DatabaseName='{0}' AND TableName='{1}'";
                else
                    statement = "INSERT INTO [E2MKI-MasterData].dbo.TD_UpdateDate (DatabaseName, TableName, UpdateDate) VALUES ('{0}', '{1}', GETDATE())";

                // - Prepare statement -
                statement = string.Format(statement, databaseNameToUpdate, tableNameUpdated);

                // - Execute Statement -
                anySqlDB.ExecuteNonQuery(statement);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during SetUpdateDate()\n{0}", e.Message);
            }
        }
        #endregion

        #region - Static method GetData -
        static public bool TryGetBool(object data)        // Boolean
        {
            return (bool)data;
        }
        static public bool TryGetBoolYesNo(object data)   // Boolean YES/NO
        {
            if (data.Equals("YES"))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Cast Object to Boolean (False returned if null or DBNULL)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public bool TryGetBoolNoNull(object data)        // Boolean
        {
            bool rBool;
            if (data == null || DBNull.Value.Equals(data))
                rBool = false;
            else
                rBool = (bool)data;
            return rBool;
        }
        static public Int32 TryGetInt32(object data)         // Int32
        {
            if (data != DBNull.Value)
                return (Int32)data;
            else
                return -1;
        }
        static public string TryGetString(object data)       // String
        {
            if (data == null || data.Equals(DBNull.Value))
                return string.Empty;
            else
                return (string)data;
        }
        static public Int32 TryGetStringInt32(object data)       // String to int32
        {
            int value = 0;
            if (data != DBNull.Value)
            {
                string tempValue = TryGetString(data);
                if (tempValue != string.Empty)
                    value = int.Parse(tempValue);
            }
            return value;
        }
        static public object TryGetDate(object data)         // Date
        {
            if (data != DBNull.Value && (string)data != "#")
                return data;
            else
                return DBNull.Value;
        }
        static public DateTime TryGetAccessDate(object data)         // Date
        {
            return (DateTime)data;
        }
        static public string TryGetDateString(object data)         // Date - String
        {
            if (data != DBNull.Value)
                //return ((DateTime)data).ToShortDateString();
                //return ((DateTime)data).ToLongDateString();
                return string.Format("{0:dd/MM/yyyy}", data);
            else
                return null;
        }
        static public Type TryGetType(object data)           // Type
        {
            return SQLType.GetType((string)data);
        }
        static public Single TryGetSingle(object data)       // Single
        {
            Single single;
            if (data != null && !data.Equals(""))
                single = Single.Parse((string)data);
            else
                single = 0;
            return single;
        }
        static public Single TryGetRealSingle(object data)       // Single
        {
            Single single;
            if (data != DBNull.Value)
                single = (Single)data;
            else
                single = 0;
            return single;
        }
        static public string TryGetSingleString(object data)    // Single String
        {
            return ((Single)data).ToString();
        }
        static public Int32 TryGetSingleInt32(object data)    // Single Int
        {
            if (data != DBNull.Value)
                return int.Parse(TryGetSingle(data).ToString());
            else
                return 0;
        }
        // --- New TryGet ---
        static public object TryGetStringDate(object data)
        {
            string dateStr = (string)data;
            if (dateStr != null) dateStr.Trim();
            if (data != DBNull.Value && dateStr != "#" && dateStr != string.Empty)
            {
                return dateStr.Replace(".", "/");
            }
            else
                return DBNull.Value;
        }
        static public Int32 TryGetStringENInt32(object data)       // String EN to int32
        {
            int value = 0;
            if (data != DBNull.Value)
            {
                string tempValue = TryGetString(data);
                if (tempValue != string.Empty)
                    value = int.Parse(tempValue.Replace(".", ""));
            }
            return value;
        }
        static public Single TryGetSingleEN(object data)       // Single English
        {
            Single single;
            if (data != null)
            {
                string tempValue = TryGetString(data);
                single = Single.Parse(tempValue.Replace(".", ""));
            }
            else
                single = 0;
            return single;
        }
        #endregion

        #region - Static method Close / FreeMemory -
        /// <summary>
        /// Clear all data and call "DataTable.Dispose()" method (E2MKI)
        /// </summary>
        /// <param name="dataTable"></param>
        static public void FreeMemory(DataTable dataTable)
        {
            if (dataTable != null)
            {
                dataTable.Clear();
                dataTable.Dispose();
                dataTable = null;
            }
        }
        static public void FreeMemory(SqlConnection sqlConn)
        {
            sqlConn.Close();
            sqlConn.Dispose();
        }
        static public void FreeMemory(SqlCommand sqlCmd)
        {
            sqlCmd.Dispose();
        }
        static public void FreeMemory(SqlCommand sqlCmd, SqlDataReader sRdr)
        {
            sqlCmd.Dispose();
            sRdr.Close();
            sRdr.Dispose();
        }
        static public void FreeMemory(SqlCommand sqlCmd, SqlConnection sqlConn)
        {
            sqlCmd.Dispose();
            sqlConn.Close();
            sqlConn.Dispose();
        }
        static public void FreeMemory(SqlCommand sqlCmd, SqlDataReader sRdr, SqlConnection sqlConn)
        {
            sqlCmd.Dispose();
            sRdr.Close();
            sRdr.Dispose();
            sqlConn.Close();
            sqlConn.Dispose();
        }
        #endregion
    }
}
