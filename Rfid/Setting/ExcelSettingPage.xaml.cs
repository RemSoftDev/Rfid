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
using System.IO;
using Rfid.Context;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for ExcelSettingPage.xaml
    /// </summary>
    public partial class ExcelSettingPage : Page
    {
        public ExcelSettingPage()
        {
            InitializeComponent();
        }

        public string PathToExcelFile { get; set; } = string.Empty;

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(btnSetFilePath, e);
            ToolTipService.SetIsEnabled(SaveButton, e);
        }             
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = folderDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    TB_Path.Text = folderDialog.SelectedPath;
                    PathToExcelFile = folderDialog.SelectedPath;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    TB_Path.Text = string.Empty;          
                    break;
            }

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PathToExcelFile != string.Empty)
            {
                Singelton.ExcelSetting.Path = PathToExcelFile;
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
            else
            {
                MessageBox.Show((string)Application.Current.Resources["msb_floderNotSelected"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
            PathToExcelFile = string.Empty;
        }
    }
}
