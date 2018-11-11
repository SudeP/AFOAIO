using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AFOAIO
{
    public class Csv : DataBase
    {
        #region Fields
        protected SaveFileDialog saveFileDialog;
        protected OpenFileDialog openFileDialog;
        private SqlDataReader sqlDataReader;
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private int RowCount;
        private int FieldCount;
        private int Outer;
        private string _Line;
        private string _FieldNames;
        private string _CommandText;
        private string[] Fields;
        #endregion
        #region Functions
        #region Write Sql To Csv
        public void SqlToCsv(string CommandText)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
            {
                Write(CommandText, this.ConnectionString);
            }
        }
        public void SqlToCsv(string CommandText, string Connectionstring)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
            {
                Write(CommandText, Connectionstring);
            }
        }
        private void Write(string CommandText, string Connectionstring)
        {
            try
            {
                base.ExecuteReader("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns WHERE TABLE_NAME = '" + GetTableName(CommandText) + "'");
                RowCount = 0;
                sqlDataReader = base.ExecuteReader(CommandText, Connectionstring);
                FieldCount = sqlDataReader.FieldCount;
                streamWriter = new StreamWriter(saveFileDialog.FileName, false, Encoding.GetEncoding(1254));
                while (sqlDataReader.Read())
                {
                    RowCount++;
                    _Line = "";
                    for (int i = 0; i < RowCount; i++)
                    {
                        Application.DoEvents();
                        Application.DoEvents();
                        _Line += sqlDataReader[i] + ";";
                    }
                    base.Log("Write : " + RowCount);
                    _Line = _Line.Remove(_Line.LastIndexOf(';'));
                    streamWriter.Write(_Line + Environment.NewLine);
                }
                streamWriter.Close();
                base.Log("Write Complated.");
            }
            catch (Exception ex)
            {
                base.Log(ex.Message);
            }
        }

        private string GetTableName(string CommandText)
        {
            CommandText = CommandText.ToLower();
            string piece = CommandText.Substring(CommandText.IndexOf("from ") + 5);
            piece = piece.Substring(0, piece.IndexOf(" "));
            return piece;
        }
        #endregion
        #region Read Csv To Sql
        #region Insert
        public void CsvToSql_Insert(string TableName)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
            {
                Read_Insert(TableName, base.ConnectionString);
            }
        }
        public void CsvToSql_Insert(string TableName, string Connectionstring)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
            {
                Read_Insert(TableName, Connectionstring);
            }
        }
        private void Read_Insert(string TableName, string Connectionstring)
        {
            try
            {
                streamReader = new StreamReader(openFileDialog.FileName, Encoding.GetEncoding(1254));
                RowCount = base.Matches(streamReader.ReadToEnd(), "\n");
                streamReader = new StreamReader(openFileDialog.FileName, Encoding.GetEncoding(1254));
                _Line = streamReader.ReadLine();
                _FieldNames = Split(_Line).Replace("'", "");
                for (int a = 1; a < RowCount; a++)
                {
                    Application.DoEvents();
                    Application.DoEvents();
                    _Line = streamReader.ReadLine();
                    _CommandText = "Insert Into " + TableName + "(" + _FieldNames + ") Values (" + Split(_Line) + ")";
                    base.ExecuteNonQuery(_CommandText, Connectionstring);
                    base.Log("Read : " + a.ToString() + "/" + (RowCount - 1));
                }
                base.Log("Read complated.");
            }
            catch (Exception ex)
            {
                base.Log(ex.Message);
            }
        }
        #endregion
        #region Update
        public void CsvToSql_Update(string TableName, string Where)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
            {
                Read_Update(TableName, Where, base.ConnectionString);
            }
        }
        public void CsvToSql_Update(string TableName, string Where, string Connectionstring)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
            {
                Read_Update(TableName, Where, Connectionstring);
            }
        }
        private void Read_Update(string TableName, string Where, string Connectionstring)
        {
            try
            {
                streamReader = new StreamReader(openFileDialog.FileName, Encoding.GetEncoding(1254));
                RowCount = base.Matches(streamReader.ReadToEnd(), "\n");
                streamReader = new StreamReader(openFileDialog.FileName, Encoding.GetEncoding(1254));
                Fields = streamReader.ReadLine().Split(';');
                for (int a = 1; a < RowCount; a++)
                {
                    Application.DoEvents();
                    Application.DoEvents();
                    _Line = streamReader.ReadLine();
                    _CommandText = "Update " + TableName + " Set " + Yerlestir(_Line.Split(';')) + ((Where != null && Where != "") ? " Where " + Where : "");
                    base.ExecuteNonQuery(_CommandText, Connectionstring);
                    base.Log("Read : " + a.ToString() + "/" + (RowCount - 1));
                }
                base.Log("Read complated.");
            }
            catch (Exception ex)
            {
                base.Log(ex.Message);
            }
        }
        #endregion
        #endregion
        #region Yardımcı Methodlar
        private string Split(string Line)
        {
            string answer = "";
            try
            {
                string[] hücreler = Line.Split(';');
                for (int i = 0; i < hücreler.Length; i++)
                {
                    Application.DoEvents();
                    Application.DoEvents();
                    answer += (int.TryParse(hücreler[i], out Outer)) ? hücreler[i] + "," : "'" + hücreler[i] + "',";
                }
            }
            catch (Exception ex)
            {
                base.Log(ex.Message);
            }
            return (answer.Remove(answer.LastIndexOf(',')));
        }
        private string Yerlestir(string[] datas)
        {
            string answer = "";
            try
            {
                for (int i = 0; i < Fields.Length; i++)
                {
                    answer += Fields[i] + "=" + ((int.TryParse(datas[i], out Outer)) ? datas[i] : " '" + datas[i] + "' ") + ",";
                }
            }
            catch (Exception ex)
            {
                base.Log(ex.Message);
            }
            return (answer.Remove(answer.LastIndexOf(',')));
        }
        #endregion
        #endregion
    }
}
