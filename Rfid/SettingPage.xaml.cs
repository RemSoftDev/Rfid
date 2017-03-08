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

namespace Rfid
{
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(TilePersonalization, e);
            ToolTipService.SetIsEnabled(TileSendingEmail, e);
            ToolTipService.SetIsEnabled(TileExcelSetting, e);
            ToolTipService.SetIsEnabled(TileLimitSetting, e);
            ToolTipService.SetIsEnabled(LanguageSetting, e);

        }

        private void TilePersonalization_Click(object sender, RoutedEventArgs e)
        {
            Singelton.MainWindow.OpenFlayout();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
        }
        private void TileSendingEmail_Click(object sender, RoutedEventArgs e)
        {
            EmailSettingPage p = new EmailSettingPage();
            Singelton.Frame.Navigate(p);
        }
        private void TileExcelSetting_Click(object sender, RoutedEventArgs e)
        {
            ExcelSettingPage p = new ExcelSettingPage();
            Singelton.Frame.Navigate(p);
        }
        private void TileLimitSetting_Click(object sender, RoutedEventArgs e)
        {
            LimitSetting p = new LimitSetting();
            Singelton.Frame.Navigate(p);
        }
        private void LanguageSetting_Click(object sender, RoutedEventArgs e)
        {
            LanguageSettingPage p = new LanguageSettingPage();
            Singelton.Frame.Navigate(p);
        }
    }
}
