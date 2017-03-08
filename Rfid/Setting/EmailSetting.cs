using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Setting
{
    class EmailSetting
    {
        public string MailFrom { get; set; } = "andriyevchak@gmail.com";
        public string Password { get; set; } = "19950512ASas!@";
        public string PathAttachmentFile { get; set; } = Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xlsx";
        public string Subject { get; set; } = "RfidNotification";
        public string Body { get; set; } = "User Statistic";
        public string SmtpClient { get; set; } = "smtp.gmail.com";       
           
    }
}
