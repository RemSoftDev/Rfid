
 using System.ComponentModel.DataAnnotations;
namespace Rfid.Models
   
{
    public class M_EmailSetting : M_Base
    {
        public string MailFrom { get; set; }
        public string Password { get; set; }
        public string PathAttachmentFile { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SmtpClient { get; set; }
        public virtual M_Setting P_Setting { get; set; }
    }
}