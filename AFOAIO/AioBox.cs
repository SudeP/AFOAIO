using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace AFOAIO
{
    #region Classes
    public class AioBox : Csv
    {
        #region Helper
        public string Helper = @"
{CONTROL_NAME} = Yazılan yere istenilen kontrolü yerleştirir

(NL) = (New Line) bir alt satıra iner
";
        #endregion
        public string A_BluePrint;
        public List<Control> A_Controls = new List<Control>();
        private MyFBox fBox;
        #region Functions
        public void A_Show(string Caption)
        {
            fBox = new MyFBox(Caption, A_BluePrint, A_Controls);
            fBox.ShowDialog();
        }
        public void A_Clear() => A_Controls.Clear();
        public string A_Answer { get => fBox.Finish; }
        public void A_Close()
        {
            fBox.Finish = "MANUALCLOSE";
            fBox.Close();
        }
        #endregion
    }
    internal class MyFBox : Form
    {
        #region Fields
        public string Finish = string.Empty;
        internal List<Control> _GetControls;
        internal string BluePrint;
        private AIO Aio = new AIO();
        private Button BtnExit;
        private Button BtnOK;
        private readonly Label LblCaption;
        private TransPanel PTop;
        private Panel PBottom;
        private Panel PBody;
        private int Frmx;
        private int Frmy;
        private const int TopControlsSize = 25;
        private const int BottomPanelSize = 35;
        private const int SpaceX = 10;
        private const int SpaceY = 10;
        #endregion
        #region Functions
        public MyFBox(string GetCaption, string bluePrint, List<Control> controls)
        {
            #region Design
            #region This
            Text = GetCaption;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.IndianRed;
            ShowInTaskbar = false;
            ShowIcon = false;
            ControlBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(125, 125);
            MaximumSize = Screen.PrimaryScreen.Bounds.Size;
            Size = MinimumSize;
            AcceptButton = BtnOK;
            CancelButton = BtnExit;
            #endregion
            #region Panel Body
            PBody = new Panel
            {
                BackColor = SystemColors.ControlLightLight,
                Location = new Point(1, 1),
                Size = new Size(Width - 2, Height - 2)
            };
            Controls.Add(PBody);
            #endregion
            #region PTop Move to This's location
            PTop = new TransPanel
            {
                Dock = DockStyle.Top,
                BackColor = SystemColors.ControlLightLight,
                BorderStyle = BorderStyle.None,
                Size = new Size(Width, TopControlsSize)
            };
            PTop.MouseDown += This_MouseDown;
            PTop.MouseMove += This_MouseMove;
            PBody.Controls.Add(PTop);
            #endregion
            #region Label Caption
            LblCaption = new Label
            {
                Text = GetCaption,
                Font = new Font("Microsoft Sans Serif", 10f),
                FlatStyle = FlatStyle.Flat,
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Black,
                Location = new Point(0, 0),
                AutoSize = true
            };
            PBody.Controls.Add(LblCaption);
            #endregion
            #region Button Exit
            BtnExit = new Button
            {
                Text = "X",
                Font = new Font("Microsoft Sans Serif", 10f)
            };
            BtnExit.FlatAppearance.MouseOverBackColor = Color.Red;
            BtnExit.FlatAppearance.MouseDownBackColor = Color.IndianRed;
            BtnExit.FlatAppearance.BorderSize = 0;
            BtnExit.FlatStyle = FlatStyle.Flat;
            BtnExit.Size = new Size(TopControlsSize, TopControlsSize);
            BtnExit.Location = new Point(0, 0);
            BtnExit.Dock = DockStyle.Right;
            BtnExit.Click += BtnExit_Click;
            PTop.Controls.Add(BtnExit);
            #endregion
            #region PBottom Control Button
            PBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(240, 240, 240),
                Size = new Size(Width, BottomPanelSize)
            };
            PBody.Controls.Add(PBottom);
            #endregion
            #region Control Buttons
            BtnOK = new Button
            {
                Name = "BtnOK",
                Text = "Tamam",
                Size = new Size(75, 25)
            };
            BtnOK.Location = new Point(PBottom.Width - BtnOK.Width - 5, ((PBottom.Height / 2) - (BtnOK.Height / 2)));
            BtnOK.FlatStyle = FlatStyle.Flat;
            BtnOK.BackColor = Color.FromArgb(240, 240, 240);
            BtnOK.FlatAppearance.MouseOverBackColor = Color.LightGray;
            BtnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(240, 240, 240);
            BtnOK.Click += BtnOk_Click;
            PBottom.Controls.Add(BtnOK);
            #endregion
            #endregion
            #region Determine
            BluePrint = bluePrint;
            _GetControls = controls;
            Determine();
            #endregion
        }
        private void Determine()
        {
            int BiggerWidth = 0, BiggerHeight = 0;
            #region Set Attribute
            string[] Rows = BluePrint.Replace("\r", "\n").Replace("\n", "").Split(new string[] { "(NL)" }, StringSplitOptions.None);
            int RowsCount = Rows.Length;
            #region Null Row
            int NullRow = 0;
            for (int i = 0; i < RowsCount; i++) { if (Rows[i] == "") NullRow++; }
            #endregion
            #region Width
            int RowinControlsCount = 0;
            for (int a = 0; a < RowsCount; a++)
            {
                RowinControlsCount = (RowinControlsCount < Aio.T_Matches(Rows[a], "{")) ? Aio.T_Matches(Rows[a], "{") : RowinControlsCount;
            }
            int RowWidth = ((RowinControlsCount * BiggerWidth) + ((RowinControlsCount + 1) * SpaceX));
            if (Width < RowWidth) Size = new Size(RowWidth, Height);
            #endregion
            #region Height
            int RowsHeight = (((RowsCount - NullRow) * BiggerHeight) + ((RowsCount + 1) * SpaceX));
            int BodyHght = (Height - PTop.Height - PBottom.Height);
            if (BodyHght < RowsHeight) Size = new Size(Width, (Height + (RowsHeight - BodyHght)));
            #endregion
            #endregion
            #region Get Attribute
            for (int i = 0; i < _GetControls.Count; i++)
            {
                if (BiggerWidth < _GetControls[i].Width) BiggerWidth = _GetControls[i].Width;
                if (BiggerHeight < _GetControls[i].Height) BiggerHeight = _GetControls[i].Height;
                if (_GetControls[i].GetType() == typeof(Label))
                {
                    ((Label)_GetControls[i]).AutoSize = true;
                    if (BiggerWidth < ((Label)_GetControls[i]).Width) BiggerWidth = ((Label)_GetControls[i]).Width;
                    if (BiggerHeight < ((Label)_GetControls[i]).Height) BiggerHeight = ((Label)_GetControls[i]).Height;
                }
            }
            #endregion
            #region Set Attribute
            Rows = BluePrint.Replace("\r", "\n").Replace("\n", "").Split(new string[] { "(NL)" }, StringSplitOptions.None);
             RowsCount = Rows.Length;
            #region Null Row
            NullRow = 0;
            for (int i = 0; i < RowsCount; i++) { if (Rows[i] == "") NullRow++; }
            #endregion
            #region Width
             RowinControlsCount = 0;
            for (int a = 0; a < RowsCount; a++)
            {
                RowinControlsCount = (RowinControlsCount < Aio.T_Matches(Rows[a], "{")) ? Aio.T_Matches(Rows[a], "{") : RowinControlsCount;
            }
             RowWidth = ((RowinControlsCount * BiggerWidth) + ((RowinControlsCount + 1) * SpaceX));
            if (Width < RowWidth) Size = new Size(RowWidth, Height);
            #endregion
            #region Height
            RowsHeight = (((RowsCount - NullRow) * BiggerHeight) + ((RowsCount + 1) * SpaceX));
             BodyHght = (Height - PTop.Height - PBottom.Height);
            if (BodyHght < RowsHeight) Size = new Size(Width, (Height + (RowsHeight - BodyHght)));
            #endregion
            #endregion
            List<string> NamesInRow;
            List<Control> DControls;
            int _w, _h = SpaceY + PTop.Height;
            bool IsHave = false;
            for (int Row = 0; Row < RowsCount; Row++)
            {
                if (Rows[Row].Contains("{") && Rows[Row].Contains("}")) // if Eleman has {BtnExpmle}
                {
                    NamesInRow = new List<string>();
                    #region Smash
                    string temporary = Rows[Row];
                    while (temporary.Contains("{") && temporary.Contains("}"))
                    {
                        string cntrl = temporary.Substring(temporary.IndexOf("{") + "{".Length, temporary.IndexOf("}") - (temporary.IndexOf("{") + "{".Length));
                        temporary = temporary.Substring(temporary.IndexOf("}"), temporary.Length - temporary.IndexOf("}"));
                        temporary = temporary.Remove(0, 1);
                        NamesInRow.Add(cntrl);
                    }
                    #endregion
                    #region Check Control|s
                    DControls = new List<Control>();
                    for (int a = 0; a < NamesInRow.Count; a++)
                    {
                        for (int b = 0; b < _GetControls.Count; b++)
                        {
                            if (_GetControls[b].Name == NamesInRow[a])
                            {
                                DControls.Add(_GetControls[b]);
                                _GetControls.Remove(_GetControls[b]);
                            }
                        }
                    }
                    if (DControls.Count == 0)
                    {
                        throw new Exception("Searched to false a control name or missing a control");//Denetim İsminin yanlış yazılması veya Kontrolün eksik olması
                    }
                    #endregion
                    #region Determine
                    if (Row > 0 && IsHave)
                        _h += SpaceY + BiggerHeight;
                    else if (Row > 0)
                    {
                        _h += SpaceY;
                        IsHave = true;
                    }
                    _w = SpaceX;
                    for (int index = 0; index < DControls.Count; index++)
                    {
                        DControls[index].Location = new Point(_w, _h);
                        DControls[index].Size = new Size(BiggerWidth, BiggerHeight);
                        PBody.Controls.Add(DControls[index]);
                        _w += SpaceX + BiggerWidth;
                    }
                    #endregion
                    #region Dispose
                    NamesInRow.Clear();
                    GC.Collect();
                    #endregion
                }
                else
                {
                    _h += SpaceY + SpaceY * 2;
                    IsHave = false;
                }
            }
            BtnOK.Location = new Point(PBottom.Width - BtnOK.Width - 5, ((PBottom.Height / 2) - (BtnOK.Height / 2)));
            BtnOK.Focus();
        }
        #region Events
        private void BtnOk_Click(object sender, EventArgs e)
        {
            Finish = "OK";
            Close();
        }
        private void This_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - Frmx;
                Top += e.Y - Frmy;
            }
        }
        private void This_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                Frmx = e.X;
                Frmy = e.Y;
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Finish = "CLOSE";
            Close();
        }
        #endregion
        #region Override
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (PBody != null)
            {
                PBody.Location = new Point(1, 1);
                PBody.Size = new Size(Width - 2, Height - 2);
            }
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Focus();
            BringToFront();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion
        #endregion
    }
    internal class TransPanel : Panel
    {
        #region Funcitons
        #region Override
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }
        #endregion
        #endregion
    }
    #endregion
}