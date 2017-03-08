using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Models.SettingModels
{
    class M_Email
    {
        public string MailFrom { get; set; }
        public string Password { get; set; }
        public string PathAttachmentFile { get; set; } 
        public string Body { get; set; } 
        public string SmtpClient { get; set; }
    }
}
