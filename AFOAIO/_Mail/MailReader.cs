using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
namespace AFOAIO._Mail
{
    public class MailReader : Csv
    {
        #region Fields
        string answer;
        TcpClient tcpClient;
        NetworkStream netStream;
        StreamReader streamReader;
        StreamWriter streamWriter;
        #endregion
        #region Functions
        #region Constractor

        #endregion
        #region Methods
        public bool M_Connect(string HostName, int Port, string User, string Password)
        {
            bool Answer = false;
            try
            {
                tcpClient = new TcpClient("mail.teknolojikbilisim.com.tr", 110)
                {
                    ReceiveTimeout = 50000,
                    SendTimeout = 50000
                };
                netStream = tcpClient.GetStream();
                streamReader = new StreamReader((Stream)tcpClient.GetStream(), Encoding.GetEncoding(1254));
                streamWriter = new StreamWriter((Stream)tcpClient.GetStream(), Encoding.GetEncoding(1254)) { AutoFlush = true };
                answer = streamReader.ReadLine();
                M_SendCommand("USER " + User);
                M_SendCommand("PASS " + Password);
            }
            catch (Exception)
            {
            }
            return Answer;
        }
        public MailCollection M_Read()
        {
            MailCollection collection = new MailCollection(new Mail[0]);
            streamWriter.Write("LIST "+Environment.NewLine);
            string count = (streamReader.ReadLine() + Environment.NewLine).Split(' ')[1];
            string next;
            do
            {
                next = streamReader.ReadLine();
                if (next == ".") break;
            } while (next != ".");
            for (int i = int.Parse(count); i >= 0; i--)
            {
                streamWriter.Write($"RETR {i}\r\n");
                string line = "";
                do
                {
                    next = streamReader.ReadLine();
                    if (next == ".")
                    {
                        break;
                    }
                    if (next != null)
                    {
                        line += next;
                        line += "\r\n";
                    }
                }
                while (line != null);
                Split(line);
            }
            return collection;
        }

        private void Split(string line)
        {

        }

        private string M_SendCommand(string Command)
        {
            streamWriter.Write(Command + Environment.NewLine);
            answer = streamReader.ReadLine();
            return answer;
        }
        public string M_GetMail(int num)
        {
            return M_SendCommand("RETR " + (object)num);
        }
        public string M_Dele(int num)
        {
            return M_SendCommand("DELE " + (object)num);
        }
        public string M_Noop()
        {
            return M_SendCommand("NOOP");
        }
        public string M_Reset()
        {
            return M_SendCommand("RSET");
        }
        #endregion
        #endregion
    }
}
