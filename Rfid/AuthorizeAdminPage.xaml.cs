using Rfid.Context;
using Rfid.Models;
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
using System.Text.RegularExpressions;
using Rfid.Sql;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for AuthorizeAdminPage.xaml
    /// </summary>
    public partial class AuthorizeAdminPage : Page
    {
       
        public AuthorizeAdminPage()
        {
            InitializeComponent();
        }

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(Authorize, e);
        }

        private void Authorize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new RfidContext())
                {
                    long rfid = Convert.ToInt64(TBGetRfidId.Text);
                    var dsd = db.Database.SqlQuery<M_Users>(new SqlSearchUserForRfid().SQl_SearchUserForRfid, rfid).SingleOrDefault();
                    Singelton.AuthorizedUser = dsd;
                }
            }
            catch
            {
                MessageBox.Show((string)Application.Current.Resources["msb_userNotFount"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StartUpPage p = new StartUpPage();
            Singelton.Frame.Navigate(p);    
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
            Keyboard.Focus(TBGetRfidId);

        }

        private void TBGetRfidId_TextChanged(object sender, TextChangedEventArgs e)
        {
            Authorize.IsEnabled = Regex.Match(TBGetRfidId.Text, @"^(\d{1,10})$").Success;
        }
    }
}
