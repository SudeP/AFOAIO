using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AFOAIO
{
     public class AIO : Csv
    {
        public AIO()
        {
            base.openFileDialog = new OpenFileDialog();
            base.saveFileDialog = new SaveFileDialog();
            base.TbxLog = new TextBox();
        }
    }
}
