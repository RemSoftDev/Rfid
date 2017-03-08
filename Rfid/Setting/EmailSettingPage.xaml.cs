using Rfid.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rfid.Helpers;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for EmailSettingPage.xaml
    /// </summary>
    public partial class EmailSettingPage : Page
    {
        public EmailSettingPage()
        {
            InitializeComponent();
        }

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(btnSaveChanges, e);
        }

        private void Button_FindUser(object sender, RoutedEventArgs e)
        {
            Singelton.EmailSetting.SmtpClient = TB_SmtpClient.Text;
            Singelton.EmailSetting.MailFrom = TB_MailFrom.Text;
            Singelton.EmailSetting.Password = StringCipher.Encrypt(TB_Password.Password, "f6v?jkfk+8#62&D=w7UW8pkfRLdxzEaKE=LH6N?q");
           
            Singelton.EmailSetting.Subject = TB_Subject.Text;
            Singelton.EmailSetting.Body = TB_Body.Text;
            using (var db = new RfidContext())
            {
                var remove = db.C_EmailSetting.FirstOrDefault();
                db.C_EmailSetting.Remove(remove);

                db.SaveChanges();


                db.C_Setting.SingleOrDefault().ExportInBdSetting();
                db.SaveChanges();
            }

            if (Singelton.Frame.CanGoBack)
            {
                Singelton.Frame.GoBack();
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            TB_SmtpClient.Text =  Singelton.EmailSetting.SmtpClient;
            TB_MailFrom.Text = Singelton.EmailSetting.MailFrom;
            TB_Password.Password = Singelton.EmailSetting.Password;
            TB_Subject.Text = Singelton.EmailSetting.Subject;
            TB_Body.Text = Singelton.EmailSetting.Body;
            Singelton.MainWindow.ChangeStringAddres(this.Title);
        }
    }
}
