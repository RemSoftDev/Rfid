using ExportToExcel;
using Rfid.Context;
using Rfid.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for GenerationAndSendingExcel.xaml
    /// </summary>
    public partial class GenerationAndSendingExcel : Page
    {
        private DataGrid _gridBasicInformation;
        private DataGrid _gridMonthlyReport;
        private DataGrid _gridDepartamentReport;    

        public GenerationAndSendingExcel()
        {
            InitializeComponent();
        }
        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(SelectFile, e);
           
        }
        public void SetGrids(DataGrid gridBasicInformation,DataGrid gridMonthlyReport,DataGrid gridDepartamentReport)
        {
            _gridBasicInformation = gridBasicInformation;
            _gridMonthlyReport = gridMonthlyReport;
            _gridDepartamentReport = gridDepartamentReport;
        } 

        private void ToolTip_Opened(object sender, RoutedEventArgs e)
        {
            ContextMenu cm;
            if (Singelton.IsToolTipEnabled)
            {
                cm = this.FindResource("CM_ExcelwithTT") as ContextMenu;
            }
            else
            {
                cm = this.FindResource("CM_Excel") as ContextMenu;
            }
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
        private void ToolTip_OpenedEmail(object sender, RoutedEventArgs e)
        {
            ContextMenu cm;
            if (Singelton.IsToolTipEnabled)
            {
                cm = this.FindResource("CM_EmailwithTT") as ContextMenu;
            }
            else
            {
                cm = this.FindResource("CM_Email") as ContextMenu;
            }
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
        private void GenerationExcelButton_Click(object sender, RoutedEventArgs e)
        {
            RfidContext db = new RfidContext();
            string path;
            if (string.IsNullOrEmpty(Singelton.ExcelSetting.Path))
            {
                MessageBox.Show(Application.Current.Resources["msb_floderNotSelected"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                 path = Singelton.ExcelSetting.Path + System.IO.Path.DirectorySeparatorChar +
                              DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xlsx";
            }
            List<TimeTableItem> TimeTable = null;
            List<UserTableItem> UserTable = UserTableItem.GenerateList(_gridBasicInformation);
            if (_gridBasicInformation.SelectedIndex != -1)
            {
                TimeTable = TimeTableItem.GenerateList(_gridMonthlyReport);
            }
            List<DepartmentTableItem> DepartmentTable = DepartmentTableItem.GenerateList(_gridDepartamentReport);




            CreateExcelFileHelper.CreateExcelDocument(UserTable, TimeTable, DepartmentTable, path);

        }
        private void SendExcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Files = new DirectoryInfo(Singelton.ExcelSetting.Path).GetFiles("*.xlsx").OrderByDescending(x => x.CreationTime);
                
                FileList.ItemsSource = Files;

                SVGenerationExcel.Visibility = Visibility.Collapsed;
                AllExcelFiles.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_floderNotSelected"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void _this_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if ((string)item.Tag == "EmailSett")
            {
                Singelton.Frame.Navigate(new EmailSettingPage());
            }
            if ((string)item.Tag == "ExcelSett")
            {
                Singelton.Frame.Navigate(new ExcelSettingPage());
            }
        }
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FileList.SelectedItem == null)
                {
                    MessageBox.Show((string)Application.Current.Resources["msb_fileNotSekected"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (TB_Email.Text == string.Empty)
                {
                    MessageBox.Show((string)Application.Current.Resources["msb_emailNotEntered"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                RfidContext db = new RfidContext();

                string path = Singelton.ExcelSetting.Path + System.IO.Path.DirectorySeparatorChar + FileList.SelectedItem.ToString();

                Singelton.EmailSetting.PathAttachmentFile = path;

                SendEmailHalper.SendEmailTo(TB_Email.Text);

                SVGenerationExcel.Visibility = Visibility.Visible;
                AllExcelFiles.Visibility = Visibility.Collapsed;
            }
            catch (FormatException)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_emailUnknownError"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
