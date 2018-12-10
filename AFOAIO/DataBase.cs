using System;
using System.Data;
using System.Data.SqlClient;
namespace AFOAIO
{
    public class DataBase : Html
    {
        #region Fields
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;
        private SqlDataAdapter sqlDataAdapter;
        private string _CommandString;
        private int answerint;
        private string connectionString;
        private bool readyConnection = false;
        private bool showQuery = false;
        public string D_ConnectionString { get => connectionString; set => connectionString = value; }
        public bool D_ReadyConnection { get => readyConnection; set => readyConnection = value; }
        public bool D_ShowQuery { get => showQuery; set => showQuery = value; }
        #endregion
        #region Functions
        #region State Control Functions
        private void Connection_Open(SqlConnection connection)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                base.T_Log(ex.Message);
                base.T_Log("Bağlantı yeniden kuruluyor...");
                if (readyConnection) Connection_Open(connection);
            }
        }
        private void Connection_Close(SqlConnection Connection)
        {
            try
            {
                if (Connection.State != System.Data.ConnectionState.Closed)
                {
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                base.T_Log(ex.Message);
            }
        }
        private void Reader_Close(SqlDataReader Reader)
        {
            if (Reader != null)
            {
                Reader.Close();
            }
        }
        private void Verify(string ConnectionString)
        {
            if (sqlCommand == null) { sqlCommand = new SqlCommand(); }
            if (sqlConnection == null) { sqlConnection = new SqlConnection(); }
            Reader_Close(sqlDataReader);
            if (sqlConnection.ConnectionString != ConnectionString)
            {
                Connection_Close(sqlConnection);
                while (sqlConnection.State != ConnectionState.Closed) T_Wait(100);
                sqlConnection.ConnectionString = ConnectionString;
            }
            Connection_Open(sqlConnection);
        }
        #endregion
        #region ExecuteNonQuery
        public int D_ExecuteNonQuery(String CommandText)
        {
            WorkBody(CommandText, D_ConnectionString, "ExecuteNonQuery");
            return answerint;
        }
        public int D_ExecuteNonQuery(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "ExecuteNonQuery");
            return answerint;
        }
        #endregion
        #region ExecuteReader
        public SqlDataReader D_ExecuteReader(String CommandText)
        {
            WorkBody(CommandText, D_ConnectionString, "SqlDataReader");
            return sqlDataReader;
        }
        public SqlDataReader D_ExecuteReader(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "SqlDataReader");
            return sqlDataReader;
        }
        public SqlDataAdapter D_ExecuteAdapter(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "SqlDataAdapter");
            return sqlDataAdapter;
        }
        #endregion
        #region ExecuteScalar
        public int D_ExecuteScalar(String CommandText)
        {
            WorkBody(CommandText, D_ConnectionString, "ExecuteScalar");
            return answerint;
        }
        public int D_ExecuteScalar(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "ExecuteScalar");
            return answerint;
        }
        #endregion
        #region Extra
        /// <summary>
        ///  ' Index = 2; ' for Tables \-.-.-/ ' Index = 0; ' for Databases
        /// </summary>
        public bool D_GetSchema(string CollectionName, string SearchWord, int ColumnIndex)
        {
            return D_GetSchema(CollectionName, SearchWord, ColumnIndex, "");
        }
        public bool D_GetSchema(string CollectionName, string SearchWord, int ColumnIndex, string ConnectionString)
        {
            return D_GetSchema(CollectionName, SearchWord, ColumnIndex, ConnectionString);
        }
        private bool GetSchema(string CollectionName, string SearchWord, int ColumnIndex, string ConnectionString)
        {
            try
            {
                Verify(ConnectionString);
                foreach (DataRow row in sqlConnection.GetSchema(CollectionName).Rows)
                {
                    if (row[ColumnIndex].ToString() == SearchWord)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                base.T_Log(ex.Message);
            }
            return false;
        }
        #endregion
        #region Main Functions
        private void WorkBody(String CommandString, String ConnectionString, String WorkName)
        {
            try
            {
            Verify:
                Verify(ConnectionString);
                if (sqlConnection.State == ConnectionState.Open) goto Verify;
                    _CommandString = CommandString;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = _CommandString;
                switch (WorkName)
                {
                    case "ExecuteNonQuery":
                        answerint = sqlCommand.ExecuteNonQuery();
                        if (showQuery) base.T_Log((CommandString.Split(' '))[0].Replace("\r\n", " ") + " işlemi gerçekleştirildi. Dönen değer/tip : " + answerint);
                        break;
                    case "SqlDataReader":
                        sqlDataReader = sqlCommand.ExecuteReader();
                        if (showQuery) base.T_Log((CommandString.Split(' '))[0].Replace("\r\n", " ") + " işlemi gerçekleştirildi. Dönen değer/tip : " + sqlDataReader.ToString());
                        break;
                    case "SqlDataAdapter":
                        sqlDataAdapter = new SqlDataAdapter(_CommandString, ConnectionString);
                        if (showQuery) base.T_Log((CommandString.Split(' '))[0].Replace("\r\n", " ") + " işlemi gerçekleştirildi. Dönen değer/tip : " + sqlDataAdapter.ToString());
                        break;
                    case "ExecuteScalar":
                        var altans = sqlCommand.ExecuteScalar();
                        answerint = altans != null ? Convert.ToInt32(altans) : 0;
                        if (showQuery) base.T_Log((CommandString.Split(' '))[0].Replace("\r\n", " ") + " işlemi gerçekleştirildi. Dönen değer/tip : " + answerint);
                        break;
                }
            }
            catch (Exception ex)
            {
                base.T_Log(ex.Message);
            }
        }
        #endregion
        #endregion
    }
}
