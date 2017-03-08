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
using System.IO.Ports;
using Rfid.Helpers;
using Rfid.Context;
using Rfid.Models;
using System.Globalization;

namespace Rfid
{
    public partial class StartUpPage : Page ,IProvideAccess
    {
        public StartUpPage()
        {
            InitializeComponent();       
            Title = "Rfid";
        }

        public static string[] checkedPort = SerialPort.GetPortNames();
        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(FindUserTile, e);
            ToolTipService.SetIsEnabled(AddUserTile, e);
            ToolTipService.SetIsEnabled(BigInfoTile, e);
            ToolTipService.SetIsEnabled(SettingTile, e);
            ToolTipService.SetIsEnabled(AuthorizeAdmin, e);
            ToolTipService.SetIsEnabled(StartTile, e);
            ToolTipService.SetIsEnabled(LogOutTile, e);
        }
        private void WriteById()
        {
        }
        public void ProvideAccess()
        {
            FindUserTile.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            AddUserTile.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            BigInfoTile.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            SettingTile.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            SettingTile.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;        
            LogOutTile.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            AuthorizeAdmin.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Collapsed : Visibility.Visible;
        }
       
        private void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
           Singelton.MainWindow.ChangeStringAddres(Title);
        }
        private void Button_User_Find(object sender, RoutedEventArgs e)
        {
            FindUserPage p = new FindUserPage();
            Singelton.Frame.NavigationService.Navigate(p);
        }
        private void Button_User_Add(object sender, RoutedEventArgs e)
        {
            var p = new AddOrUpdateUserPage();
            Singelton.Frame.NavigationService.Navigate(p);
        }
        private void Button_RfidStart(object sender, RoutedEventArgs e)
        {
            GetRFIDIPage p = new GetRFIDIPage();
            Singelton.Frame.NavigationService.Navigate(p);
        }
        private void Button_BigInfo(object sender, RoutedEventArgs e)
        {
            var p = new ReportPage();
            base.NavigationService.Navigate(p);
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            ReportPage p = new ReportPage();
            base.NavigationService.Navigate(p);
        }
        private void SettingTile_Click(object sender, RoutedEventArgs e)
        {
            SettingPage p = new SettingPage();
            base.NavigationService.Navigate(p);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
            ProvideAccess();
        }
        private void AuthorizeAdmin_Click(object sender, RoutedEventArgs e)
        {
            AuthorizeAdminPage p = new AuthorizeAdminPage();
            Singelton.Frame.Navigate(p);
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Singelton.AuthorizedUser = null;
            ProvideAccess();
        }
    }

}
