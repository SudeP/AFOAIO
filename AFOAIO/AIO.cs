using System.Windows.Forms;

namespace AFOAIO
{
    public class AIO : AioBox
    {
        public AIO()
        {
            base.saveFileDialog = new SaveFileDialog();
            base.openFileDialog = new OpenFileDialog();
            base.T_TbxLog = new TextBox { Name = "NULL", Text = "MULL" };
        }
    }
}
