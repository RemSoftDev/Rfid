using System;
using System.Net;
using System.Net.Mail;
using System.Windows;
using Rfid.Helpers;

namespace Rfid.Helpers
{
    public static class SendEmailHalper
    {
        public static void email_send(string MailFrom , string Password, string MailTo, string Path, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
            /* здесь указываете SMTP и Порт, у меня например mail.ru - я 
указал smtp.mail.ru, а порт smtp.mail.ru - 25 или 2525 */
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(MailFrom);
            mail.To.Add(MailTo);
            mail.Subject = Subject;
            mail.Body = Body;
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(Path);
            mail.Attachments.Add(attachment);
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(MailFrom, StringCipher.Decrypt(Password, "SomeVeryLongAndSecurePassphrase"));
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

        public static void SendEmailTo(string mailTo)
        {
            try
            {
                MailMessage mail = new MailMessage();
                /* здесь указываете SMTP и Порт, у меня например mail.ru - я 
указал smtp.mail.ru, а порт smtp.mail.ru - 25 или 2525 */
                SmtpClient SmtpServer = new SmtpClient(Singelton.EmailSetting.SmtpClient);
                mail.From = new MailAddress(Singelton.EmailSetting.MailFrom);
                mail.To.Add(mailTo);
                mail.Subject = Singelton.EmailSetting.Subject;
                mail.Body = Singelton.EmailSetting.Body;
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(Singelton.EmailSetting.PathAttachmentFile);
                mail.Attachments.Add(attachment);
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Singelton.EmailSetting.MailFrom,
                    StringCipher.Decrypt(Singelton.EmailSetting.Password, "f6v?jkfk+8#62&D=w7UW8pkfRLdxzEaKE=LH6N?q"));
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
