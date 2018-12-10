using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AFOAIO._Mail
{
    class Mail
    {
        public string M_From { get; set; }
        public List<string> M_To { get; set; }
        public string M_Subject { get; set; }
        public string M_Body { get; set; }
        public AttachmentCollection M_Attachments { get; set; }
        public int M_MailId { get; set; }
    }
}
