using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;


namespace DataAccess
{
    /// <summary>
    /// SQL Server DataBase.
    /// </summary>
    public class SQLDataBase
    {
        #region - Constants -
        /// <summary>
        /// Time before terminating the attempt to execute a command.
        /// </summary>
        private const int COMMAND_TIMEOUT_QUERY = 300;      //-> 300 seconds (5 minutes)
        private const int COMMAND_TIMEOUT_PROCEDURE = 1200;  //-> 1200 seconds (10 minutes)
        #endregion

        #region - Attributes -
        private static Dictionary<int, SqlCommand> openedCmd;
        private static Dictionary<int, SqlConnection> openedConn;
        private static IFormatProvider formatProvider;
        private static string assemblyName = null;

        public string serverName;
        private string databaseName;
        private string connectionString;
        #endregion

        #region - Constructor -
        /// <summary>
        /// SQL Server DataBase.
        /// </summary>
        static SQLDataBase()
        {
            openedCmd = new Dictionary<int, SqlCommand>();
            openedConn = new Dictionary<int, SqlConnection>();
            formatProvider = CultureInfo.GetCultureInfo("en-US").NumberFormat;  //-> SQL Server works in en-US

            // - Try get assembly name -
            assemblyName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? ".Net Application";
        }

        /// <summary>
        /// SQL Server DataBase.
        /// </summary>
        public SQLDataBase(string serverName, string databaseName)
        {
            this.serverName = serverName;
            this.databaseName = databaseName;
            connectionString = "Data Source={0};Initial Catalog={1};Application Name={2};Connection Timeout=90;Integrated Security=True";
            connectionString = string.Format(connectionString, serverName, databaseName, assemblyName);
        }

        /// <summary>
        /// SQL Server DataBase.
        /// </summary>
        public SQLDataBase(string serverName, string databaseName, string userID, string password)
        {
            this.serverName = serverName;
            this.databaseName = databaseName;
            connectionString = "Data Source={0};Initial Catalog={1};Application Name={2};Connection Timeout=90;User Id={3};Password={4};";
            connectionString = string.Format(connectionString, serverName, databaseName, assemblyName, userID, password);
        }
        #endregion

        #region - Properties -
        public string ConnectionString
        {
            get { return connectionString; }
        }
        /// <summary>
        /// Name of the DataBase in SQL Server.
        /// </summary>
        public string Name
        {
            get { return databaseName; }
        }
        #endregion

        #region - Function -

        #region - Connection -
        public SqlConnection GetNewConnection()
        {
            return new SqlConnection(connectionString);
        }
        public SqlConnection GetNewOpenedConnection()
        {
            SqlConnection sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            return sqlConn;
        }
        #endregion

        public void FillDataTable(string query, DataTable dataTable)
        {
            // - Execute query -
            SqlDataReader sDR = ExecuteReader(query);

            // - Load results into the datable -
            dataTable.Load(sDR);

            // - Free memory -
            CloseOpenedQuery(sDR);
        }

        #region - Get DataTable -
        public DataTable GetDataTable(string query, params object[] args)
        {
            return GetDataTable(string.Format(formatProvider, query, args));
        }

        public DataTable GetDataTable(string query)
        {
            // - Init -
            DataTable dataTable = new DataTable();

            SqlDataReader sDR = ExecuteReader(query);

            // - Building datatable -
            for (int i = 0; i < sDR.FieldCount; i++)
            {
                dataTable.Columns.Add(new DataColumn(sDR.GetName(i), SQLType.GetType(sDR.GetDataTypeName(i))));
            }

            // - Start loading data (remove all index, constraints...) -
            dataTable.BeginLoadData();

            // - Fill datatable -
            dataTable.Load(sDR, LoadOption.OverwriteChanges);

            // - Stop loading data (Re-Apply all index, constraints...) -
            dataTable.EndLoadData();

            // - Prevent datatable storing several versions of rows -
            dataTable.AcceptChanges();

            // - Free memory -
            CloseOpenedQuery(sDR);

            // - Return result -
            return dataTable;
        }

        /// <summary>
        /// Return data from SQL Server.
        /// </summary>
        /// <param name="query">Query text.</param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetSingleResult(string cmdText)
        {
            return GetSingleResult(cmdText, 0);
        }
        /// <summary>
        /// Return data from SQL Server.
        /// </summary>
        /// <param name="query">Query text.</param>
        /// <param name="query">Minimum capacity.</param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetSingleResult(string cmdText, int minimumCapacity)
        {
            // - Build SQL Connection -
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // - Open SQL Connection -
                sqlConnection.Open();

                // - Build SQL command -
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection))
                {
                    // - Set command timeout -
                    sqlCommand.CommandTimeout = COMMAND_TIMEOUT_QUERY;

                    // - Run stored procedure -
                    using (SqlDataReader sdReader = sqlCommand.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        // - Retrieve field count -
                        int fieldCount = sdReader.FieldCount;

                        // - Building datatable -
                        DataTable dataTable = new DataTable();
                        if (minimumCapacity > 0) dataTable.MinimumCapacity = minimumCapacity;

                        // - Fill columns -
                        for (int i = 0; i < fieldCount; i++)
                            dataTable.Columns.Add(new DataColumn(sdReader.GetName(i), sdReader.GetFieldType(i)));

                        // - Read each line -
                        while (sdReader.Read())
                        {
                            DataRow dRow = dataTable.NewRow();
                            for (int i = 0; i < fieldCount; i++)
                            {
                                dRow[i] = sdReader[i];
                            }
                            dataTable.Rows.Add(dRow);
                        }

                        // - Return data result set -
                        return dataTable;
                    }
                }
            }
        }
        public DataTable GetSingleResultBeginEnd(string cmdText, int minimumCapacity)
        {
            // - Build SQL Connection -
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // - Open SQL Connection -
                sqlConnection.Open();

                // - Build SQL command -
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection))
                {
                    // - Set command timeout -
                    sqlCommand.CommandTimeout = COMMAND_TIMEOUT_QUERY;

                    // - Run stored procedure -
                    using (SqlDataReader sdReader = sqlCommand.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        // - Retrieve field count -
                        int fieldCount = sdReader.FieldCount;

                        // - Building datatable -
                        DataTable dataTable = new DataTable() { MinimumCapacity = minimumCapacity };

                        dataTable.BeginLoadData();

                        // - Fill columns -
                        for (int i = 0; i < fieldCount; i++)
                            dataTable.Columns.Add(new DataColumn(sdReader.GetName(i), sdReader.GetFieldType(i)));

                        // - Read each line -
                        while (sdReader.Read())
                        {
                            DataRow dRow = dataTable.NewRow();

                            for (int i = 0; i < fieldCount; i++)
                                dRow[i] = sdReader[i];

                            dataTable.Rows.Add(dRow);
                        }

                        dataTable.EndLoadData();

                        // - Return data result set -
                        return dataTable;
                    }
                }
            }
        }
        public DataTable GetEmptyResult(string cmdText, params string[] args)
        {
            return GetEmptyResult(string.Format(cmdText, args));
        }

        public DataTable GetEmptyResult(string cmdText)
        {
            // - Build SQL Connection -
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // - Open SQL Connection -
                sqlConnection.Open();

                // - Build SQL command -
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection))
                {
                    // - Set command timeout -
                    sqlCommand.CommandTimeout = COMMAND_TIMEOUT_QUERY;

                    // - Run stored procedure -
                    using (SqlDataReader sdReader = sqlCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo | CommandBehavior.SingleResult))
                    {
                        // - Building datatable -
                        DataTable dataTable = new DataTable();

                        // - Load data into the DataTable (schema only) -
                        dataTable.Load(sdReader, LoadOption.OverwriteChanges);

                        //// - Fill columns -
                        //for (int i = 0; i < sdReader.FieldCount; i++)
                        //    dataTable.Columns.Add(new DataColumn(sdReader.GetName(i), sdReader.GetFieldType(i)));

                        // - Return data result set -
                        return dataTable;
                    }
                }
            }
        }

        /// <summary>
        /// Return data from SQL Server.
        /// </summary>
        /// <param name="query">Query text.</param>
        /// <param name="query">Minimum capacity.</param>
        /// <returns>System.Data.DataTable</returns>
        public List<DataTable> GetMultiResult(string cmdText, params object[] args)
        {
            // - Build SQL Connection -
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // - Open SQL Connection -
                sqlConnection.Open();

                // - Build SQL command -
                using (SqlCommand sqlCommand = new SqlCommand(string.Format(cmdText, args), sqlConnection))
                {
                    // - Set command timeout -
                    sqlCommand.CommandTimeout = COMMAND_TIMEOUT_QUERY;

                    // - Run stored procedure -
                    using (SqlDataReader sdReader = sqlCommand.ExecuteReader(CommandBehavior.Default))
                    {
                        // - Build DataTable list -
                        List<DataTable> resultList = new List<DataTable>();

                        // - Iterate through results -
                        do
                        {
                            // - Retrieve field count -
                            int fieldCount = sdReader.FieldCount;

                            // - Building datatable -
                            DataTable dataTable = new DataTable();

                            // - Fill columns -
                            for (int i = 0; i < fieldCount; i++)
                                dataTable.Columns.Add(new DataColumn(sdReader.GetName(i), sdReader.GetFieldType(i)));

                            // - Read each line -
                            while (sdReader.Read())
                            {
                                DataRow dRow = dataTable.NewRow();
                                for (int i = 0; i < fieldCount; i++)
                                    dRow[i] = sdReader[i];
                                dataTable.Rows.Add(dRow);
                            }

                            // - Add DataTable to the result list -
                            resultList.Add(dataTable);
                        }
                        while (sdReader.NextResult());

                        // - Return data result set -
                        return resultList;
                    }
                }
            }
        }
        #endregion

        #region - Get DataRow -
        public DataRow GetDataRow(string query, params object[] args)
        {
            return GetDataRow(string.Format(formatProvider, query, args));
        }
        public DataRow GetDataRow(string query)
        {
            using (DataTable resultTable = GetDataTable(query))
                return resultTable.Rows.Count == 0 ? null : resultTable.Rows[0];
        }
        #endregion

        #region - Run stored procedure -
        /// <summary>
        /// Run stored procedure without parameters, no results.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure to run.</param>
        public void RunStoredProcedure(string storedProcedureName)
        {
            // - Declare message -
            string outputMessage = null;

            // - Run stored procedure -
            RunStoredProcedure(storedProcedureName, out outputMessage);
        }

        /// <summary>
        /// Run stored procedure without parameters, no results.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure to run.</param>
        public void RunStoredProcedure(string storedProcedureName, out string outputMessage)
        {
            // - Build SQL connection -
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // - Initialize message -
                string localMessage = string.Empty;

                // - Set message handler -
                sqlConnection.InfoMessage += (sender, infoMessage) => localMessage += infoMessage.Message;

                // - Open connection -
                sqlConnection.Open();

                // - Build SQL command -
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    // - Set command type -
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // - Set command timeout -
                    sqlCommand.CommandTimeout = COMMAND_TIMEOUT_PROCEDURE;

                    // - Run stored procedure -
                    sqlCommand.ExecuteNonQuery();

                    // - Set output message -
                    outputMessage = localMessage;
                }
            }
        }

        /// <summary>
        /// Run stored procedure without parameters, scalar return.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure to run.</param>
        public object RunStoredProcedureScalar(string storedProcedureName)
        {
            // - Build SQL connection -
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // - Open connection -
                sqlConnection.Open();

                // - Build SQL command -
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    // - Set command type -
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // - Set command timeout -
                    sqlCommand.CommandTimeout = COMMAND_TIMEOUT_PROCEDURE;

                    // - Run stored procedure -
                    return sqlCommand.ExecuteScalar();
                }
            }
        }
        #endregion

        #region - Generic functions -
        public SqlDataReader ExecuteReader(string statement)
        {
            SqlConnection sqlConn = null; SqlCommand sqlCmd = null; SqlDataReader result;
            try
            {
                sqlConn = GetNewOpenedConnection();
                sqlCmd = new SqlCommand(statement, sqlConn);
                sqlCmd.CommandTimeout = 60;
                result = sqlCmd.ExecuteReader();
                // - Saving opened connection and command -
                openedCmd.Add(result.GetHashCode(), sqlCmd);
                openedConn.Add(result.GetHashCode(), sqlConn);
            }
            catch (Exception e)
            {
                SQLTable.FreeMemory(sqlCmd, sqlConn);
                throw e;
            }
            return result;
        }
        public object ExecuteScalar(string query, params object[] args)
        {
            return ExecuteScalar(string.Format(query, args));
        }
        public object ExecuteScalar(string statement)
        {
            SqlConnection sqlConn = null; SqlCommand sqlCmd = null; object result;
            try
            {
                sqlConn = GetNewOpenedConnection();
                sqlCmd = new SqlCommand(statement, sqlConn);
                sqlCmd.CommandTimeout = 60;
                result = sqlCmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                SQLTable.FreeMemory(sqlCmd, sqlConn);
            }
            return result;
        }
        public int ExecuteNonQuery(string statement, params object[] args)
        {
            return ExecuteNonQuery(string.Format(formatProvider, statement, args));
        }

        public int ExecuteNonQuery(string statement)
        {
            SqlConnection sqlConn = null; SqlCommand sqlCmd = null; int result;
            try
            {
                sqlConn = GetNewOpenedConnection();
                sqlCmd = new SqlCommand(statement, sqlConn);
                sqlCmd.CommandTimeout = 60;
                result = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                SQLTable.FreeMemory(sqlCmd, sqlConn);
            }
            return result;
        }

        public int ExecuteNonQueryLong(string statement, params object[] args)
        {
            // - Build SQL connection -
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // - Open connection -
                sqlConnection.Open();

                // - Build SQL command -
                using (SqlCommand sqlCommand = new SqlCommand(string.Format(statement, args), sqlConnection))
                {
                    // - Set command timeout -
                    sqlCommand.CommandTimeout = 0;  //-> 0 means infinite

                    // - Run long command -
                    return sqlCommand.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #endregion

        #region - Static Functions -
        public void CloseOpenedQuery(SqlDataReader sDR)
        {
            // - Init -
            int hashCode = sDR.GetHashCode();

            // - Close SqlDataReader -
            sDR.Close();
            sDR.Dispose();

            // - Get and close SqlCommand -
            if (openedCmd.ContainsKey(hashCode))
            {
                SqlCommand sqlCmd = openedCmd[hashCode];
                openedCmd.Remove(hashCode);
                sqlCmd.Dispose();
            }

            // - Get and close SqlConnection -
            if (openedConn.ContainsKey(hashCode))
            {
                SqlConnection sqlConn = openedConn[hashCode];
                openedConn.Remove(hashCode);
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }
        #endregion
    }
}
