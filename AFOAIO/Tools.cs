using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AFOAIO
{
    public class Tools : LocalWriter
    {
        #region Fields
        public TextBox TbxLog;
        private int answer;
        #endregion
        #region Funtions
        public void Log(string txt)
        {
            if (TbxLog.Text.Length > 5000) { TbxLog.Text = string.Empty; }
            TbxLog.Text += DateTime.Now.ToString("dd MMM") + " - " + DateTime.Now.ToString("HH:mm:ss") + "  --  " + txt + Environment.NewLine;
            TbxLog.SelectionStart = TbxLog.TextLength;
            TbxLog.ScrollToCaret();
        }
        public void Wait(int sure)
        {
            DateTime t = DateTime.Now.AddMilliseconds(sure);
            int i = 0;
            while (DateTime.Now < t)
            {
                if (i % 2 == 0)
                    Application.DoEvents();
                i++;
            }
        }
        public string HtmlTextClear(string text)
        {
            text = text.Replace("'", " ");
            text = text.Replace("\"", " ");
            text = text.Replace("&nbsp;", " ");
            text = text.Replace("&#8217;", " ");
            text = text.Replace("&#8221;", " ");
            text = text.Replace("&#8221;", " ");
            text = text.Replace("&#8211;", " ");
            text = text.Replace("&#8220;", " ");
            text = text.Replace("&#39;", "'");
            return text;
        }
        public int Matches(String Text, String Piece)
        {
            answer = 0;
            Text = Text.ToLower();
            while (Text.Contains(Piece))
            {
                Text = Text.Substring(Text.IndexOf(Piece) + 1);
                answer++;
            }
            return answer;
        }
        #endregion
    }
}
