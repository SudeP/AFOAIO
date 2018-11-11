using System;
using System.IO;
using System.Windows.Forms;
namespace AFOAIO
{
    public class LocalWriter
    {
        #region Fields
        protected string LocalLogFolderName = string.Empty;
        private string MonthFolder;
        private string DayFolder;
        private string NowFolder;
        private readonly string NowFolderType = ".txt";
        private String AppPath;
        private StreamWriter streamWriter;
        private DateTime dateTime;
        #endregion
        #region Functions
        public void LocalLog(String Log)
        {
            AppPath = Application.StartupPath;
            DetermineDate();
            ControlFolder();
            if (!File.Exists(AppPath + "\\" + LocalLogFolderName + "\\" + MonthFolder + "\\" + DayFolder + "\\" + NowFolder + NowFolderType))
            {
                streamWriter = File.CreateText(AppPath + "\\" + LocalLogFolderName + "\\" + MonthFolder + "\\" + DayFolder + "\\" + NowFolder + ".txt");
            }
            else
            {
                streamWriter = File.AppendText(AppPath + "\\" + LocalLogFolderName + "\\" + MonthFolder + "\\" + NowFolder + NowFolderType);
            }
            streamWriter.Write(Log + Environment.NewLine);
            streamWriter.Close();
        }
        private void DetermineDate()
        {
            dateTime = DateTime.Now;
            MonthFolder = dateTime.ToString("MM.yyyy");
            DayFolder = dateTime.Day.ToString();
            NowFolder = dateTime.ToString("HH.mm");
        }
        private void ControlFolder()
        {
            if (!Directory.Exists(AppPath + "\\" + LocalLogFolderName + ""))
            {
                Directory.CreateDirectory(AppPath + "\\" + LocalLogFolderName + "");
            }
            if (!Directory.Exists(AppPath + "\\" + LocalLogFolderName + "\\" + MonthFolder))
            {
                Directory.CreateDirectory(AppPath + "\\" + LocalLogFolderName + "\\" + MonthFolder);
            }
            if (!Directory.Exists(AppPath + "\\" + LocalLogFolderName + "\\" + MonthFolder + "\\" + DayFolder))
            {
                Directory.CreateDirectory(AppPath + "\\" + LocalLogFolderName + "\\" + MonthFolder + "\\" + DayFolder);
            }
        }
        #endregion
    }
}