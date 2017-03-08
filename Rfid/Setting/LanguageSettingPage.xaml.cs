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

namespace Rfid
{
    /// <summary>
    /// Interaction logic for LanguageSettingPage.xaml
    /// </summary>
    public partial class LanguageSettingPage : Page
    {
        public LanguageSettingPage()
        {
            InitializeComponent();
        }

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(SaveButton, e);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Singelton.LanguageSetting.SelectedLanguage = CB_ChangeLanguage.SelectedIndex;

            using (var db = new RfidContext())
            {
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
            Singelton.MainWindow.ChangeStringAddres(Title);
            CB_ChangeLanguage.SelectedIndex = Singelton.LanguageSetting.SelectedLanguage;
        }
    }
}
