using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFOAIO
{
    public class DataBase : Html
    {
        #region Fields
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;
        private SqlDataAdapter sqlDataAdapter;
        private string connectionString;
        private string _Komutmetni;
        private int answerint;
        public string ConnectionString { get => connectionString; set => connectionString = value; }
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
                base.Log(ex.Message);
                base.Log("Bağlantı yeniden kuruluyor...");
                Connection_Open(connection);
            }
        }
        private void Connection_Close(SqlConnection Connection)
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                base.Log(ex.Message);
            }
        }
        private void Reader_Close(SqlDataReader Reader)
        {
            if (Reader != null)
            {
                Reader.Close();
            }
        }
        private void Verify()
        {
            if (sqlCommand == null) { sqlCommand = new SqlCommand(); }
            if (sqlConnection == null) { sqlConnection = new SqlConnection(); }
            Connection_Close(sqlConnection);
            sqlConnection.ConnectionString = ConnectionString;
            Connection_Open(sqlConnection);
        }
        #endregion
        #region ExecuteNonQuery
        public int ExecuteNonQuery(String CommandText)
        {
            WorkBody(CommandText, ConnectionString, "ExecuteNonQuery");
            return answerint;
        }
        public int ExecuteNonQuery(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "ExecuteNonQuery");
            return answerint;
        }
        #endregion
        #region ExecuteReader
        public SqlDataReader ExecuteReader(String CommandText)
        {
            WorkBody(CommandText, ConnectionString, "SqlDataReader");
            return sqlDataReader;
        }
        public SqlDataReader ExecuteReader(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "SqlDataReader");
            return sqlDataReader;
        }
        public SqlDataAdapter ExecuteAdapter(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "SqlDataAdapter");
            return sqlDataAdapter;
        }
        #endregion
        #region ExecuteScalar
        public int ExecuteScalar(String CommandText)
        {
            WorkBody(CommandText, ConnectionString, "ExecuteScalar");
            return answerint;
        }
        public int ExecuteScalar(String CommandText, String ConnectionString)
        {
            WorkBody(CommandText, ConnectionString, "ExecuteScalar");
            return answerint;
        }
        #endregion
        #region Extra
        /// <summary>
        ///  ' Index = 2; ' for Tables - ' Index = 0; ' for Databases
        /// </summary>
        public bool GetSchema(string CollectionName, string SearchWord, int ColumnIndex)
        {
            try
            {
                Verify();
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
                base.Log(ex.Message);
            }
            return false;
        }
        #endregion
        #region Main Functions
        private void WorkBody(String Komutmetni, String ConnectionString, String WorkName)
        {
            try
            {
                Verify();
                _Komutmetni = Komutmetni;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = _Komutmetni;
                switch (WorkName)
                {
                    case "ExecuteNonQuery":
                        answerint = sqlCommand.ExecuteNonQuery();
                        base.Log((Komutmetni.Split(' '))[0] + " işlemi gerçekleştirildi. Dönen değer/tip : " + answerint);
                        break;
                    case "SqlDataReader":
                        sqlDataReader = sqlCommand.ExecuteReader();
                        base.Log((Komutmetni.Split(' '))[0] + " işlemi gerçekleştirildi. Dönen değer/tip : " + sqlDataReader.ToString());
                        break;
                    case "SqlDataAdapter":
                        sqlDataAdapter = new SqlDataAdapter(_Komutmetni, ConnectionString);
                        base.Log((Komutmetni.Split(' '))[0] + " işlemi gerçekleştirildi. Dönen değer/tip : " + sqlDataAdapter.ToString());
                        break;
                    case "ExecuteScalar":
                        var altans = sqlCommand.ExecuteScalar();
                        answerint = altans != null ? Convert.ToInt32(altans) : 0;
                        base.Log((Komutmetni.Split(' '))[0] + " işlemi gerçekleştirildi. Dönen değer/tip : " + answerint);
                        break;
                }
            }
            catch (Exception ex)
            {
                base.Log(ex.Message);
            }
        }
        #endregion
        #endregion
    }
}
