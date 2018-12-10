using System;
using System.IO;
using System.Windows.Forms;
namespace AFOAIO
{
    public class LocalWriter
    {
        #region Fields
        private string localLogFolderName = string.Empty;
        private string MonthFolder;
        private string DayFolder;
        private string NowFolder;
        private readonly string NowFolderType = ".txt";
        private String AppPath;
        private StreamWriter streamWriter;
        private DateTime dateTime;
        public string L_LocalLogFolderName { get => localLogFolderName; set => localLogFolderName = value; }
        #endregion
        #region Functions
        /// <summary>
        /// Auto Create Folder. Format : Debug/[localLogFolderName]/Month/Day/Now.txt
        /// </summary>
        public void L_LocalLog(string Log)
        {
            AppPath = Application.StartupPath;
            DetermineDate();
            ControlFolder();
            if (!File.Exists(AppPath + "\\" + localLogFolderName + "\\" + MonthFolder + "\\" + DayFolder + "\\" + NowFolder + NowFolderType))
            {
                streamWriter = File.CreateText(AppPath + "\\" + localLogFolderName + "\\" + MonthFolder + "\\" + DayFolder + "\\" + NowFolder + ".txt");
            }
            else
            {
                streamWriter = File.AppendText(AppPath + "\\" + localLogFolderName + "\\" + MonthFolder + "\\" + NowFolder + NowFolderType);
            }
            streamWriter.Write(Log + Environment.NewLine);
            streamWriter.Close();
        }
        public void L_LocalLog(string Folder, string SubFolder, string StepName, string Log)
        {
            AppPath = System.Windows.Forms.Application.StartupPath;
            ControlFolder(Folder, SubFolder);
            if (!File.Exists(AppPath + "\\" + localLogFolderName + "\\" + Folder + "\\" + SubFolder + "\\" + StepName + NowFolderType))
                streamWriter = File.CreateText(AppPath + "\\" + localLogFolderName + "\\" + Folder + "\\" + SubFolder + "\\" + StepName + NowFolderType);
            else
                streamWriter = File.AppendText(AppPath + "\\" + localLogFolderName + "\\" + Folder + "\\" + SubFolder + "\\" + StepName + NowFolderType);
            streamWriter.Write(DateTime.Now + " " + Log + Environment.NewLine);
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
            if (!Directory.Exists(AppPath + "\\" + localLogFolderName + ""))
            {
                Directory.CreateDirectory(AppPath + "\\" + localLogFolderName + "");
            }
            if (!Directory.Exists(AppPath + "\\" + localLogFolderName + "\\" + MonthFolder))
            {
                Directory.CreateDirectory(AppPath + "\\" + localLogFolderName + "\\" + MonthFolder);
            }
            if (!Directory.Exists(AppPath + "\\" + localLogFolderName + "\\" + MonthFolder + "\\" + DayFolder))
            {
                Directory.CreateDirectory(AppPath + "\\" + localLogFolderName + "\\" + MonthFolder + "\\" + DayFolder);
            }
        }
        private void ControlFolder(string Folder, string SubFolder)
        {
            if (!Directory.Exists(AppPath + "\\" + localLogFolderName + ""))
                Directory.CreateDirectory(AppPath + "\\" + localLogFolderName);
            if (!Directory.Exists(AppPath + "\\" + localLogFolderName + "\\" + Folder))
                Directory.CreateDirectory(AppPath + "\\" + localLogFolderName + "\\" + Folder);
            if (!Directory.Exists(AppPath + "\\" + localLogFolderName + "\\" + Folder + "\\" + SubFolder))
                Directory.CreateDirectory(AppPath + "\\" + localLogFolderName + "\\" + Folder + "\\" + SubFolder);
        }
        #endregion
    }
}