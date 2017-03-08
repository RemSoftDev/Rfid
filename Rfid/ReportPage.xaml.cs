using DocumentFormat.OpenXml.Drawing.Charts;
using ExportToExcel;
using Rfid.Context;
using Rfid.Helpers;
using Rfid.Models;
using Rfid.Sql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Threading;

namespace Rfid
{
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
         
            InitializeComponent();
            RfidContext db = new RfidContext();
            GridBasicInformation.EnableColumnVirtualization = true;
            GridBasicInformation.Items.Clear();
            GridBasicInformation.CanUserAddRows = false;
            GridBasicInformation.ItemsSource = db.
                                                Database.
                                                SqlQuery<BasicInformation>((new SqlBasicInformation()).SQl_BasicInformation).
                                                ToList();
            GridBasicInformation.EnableColumnVirtualization = true;
            GridBasicInformation.Items.Refresh();
            var a = GridBasicInformation.CommandBindings;
            var queryAllDepartamentsNames = from cust in db.C_Departments
                                            where cust.ID != 1
                                            select new { cust.Name };
            List<DepartamentsData> listDepInfo = new List<DepartamentsData>();

            foreach (var depName in queryAllDepartamentsNames.ToList())
            {
                DepartamentsData temp = getDepartamentClientData(depName.Name);
                listDepInfo.Add(temp);
            }

            GridDepartamentReport.ItemsSource = listDepInfo;
            this.GridDepartamentReport.Items.Refresh();
            var d = GridDepartamentReport.Columns;
            SelectedDepartamentReport_MouseLeftButtonUp(null, null);
            SelectedMonthlyReport_MouseLeftButtonUp(null, null);
            SelectedBasicInformation_MouseLeftButtonUp(null, null);
        }
        public ReportPage(int selected): this()
        {
            SelectedId = selected;            
        }

        private int? SelectedId;
        private M_Users SelectedUser;
        private bool _isSearch;
        public CalendarType CalendarTypeSetting { get; set; }

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(SelectedBasicInformation, e);
            ToolTipService.SetIsEnabled(SelectedMonthlyReport, e);
            ToolTipService.SetIsEnabled(SelectedDepartamentReport, e);
            ToolTipService.SetIsEnabled(SelectedAllTime, e);
            ToolTipService.SetIsEnabled(GenerationExcel, e);
            ToolTipService.SetIsEnabled(CalendarWeekend, e);
        }
        private DepartamentsData getDepartamentClientData(string name)
        {

                RfidContext db = new RfidContext();

                var queryAllDepartaments = from cust in db.C_Names
                                           where cust.P_Users.P_Departments.Name.Equals(name)
                                           select new { cust.P_Users.IsUser, cust.P_Users.isInside }
                                     ;
                var t1 = queryAllDepartaments.Count();
                var t2 = queryAllDepartaments.Where(x => x.isInside == true).Count();
                var t3 = queryAllDepartaments.Where(x => x.isInside == false).Count();

                DateTime date = DateTime.Now;

                DepartamentsData departParam = new DepartamentsData(date.ToString("dd.MM.yyyy"), name, t1, t2, t3);

                return departParam;
            
            
        }
        public void HideGrids()
        {
            ViewBasicInformation.Visibility = Visibility.Collapsed;
            ViewMonthlyReport.Visibility = Visibility.Collapsed;
            ViewDepartamentReport.Visibility = Visibility.Collapsed;
            GridAllTime.Visibility = Visibility.Collapsed;
            SVGenerationExcel.Visibility = Visibility.Collapsed;
            AllExcelFiles.Visibility = Visibility.Collapsed;
            CalendarAndWeekend.Visibility = Visibility.Collapsed;
        }
        private void CalendarWeekend_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HideGrids();
            CalendarAndWeekend.Visibility = Visibility.Visible;
            var p = new CalendarAndWeekendPage();
            CalendarFrame.Navigate(p);
            p.SetInfoAboutSelectedUser(SelectedId, SelectedUser);
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["Holidays"]);
        }
        private void GridBasicInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridBasicInformation.SelectedIndex == -1)
            {
                return;
            }

            if (!_isSearch)
            {
                dynamic s = GridBasicInformation.CurrentCell.Item;
                SelectedId = s.ID;
            }

            RfidContext db = new RfidContext();
            SelectedUser = (from d in db.C_Users
                            where d.ID == SelectedId
                            select d).Single();
            var culture = App.Language;

            if(SelectedUser.P_UserTime.ToList().Count != 0)
            {
                var queryAllCustomers = (from cust in db.C_UserTime
                                        where cust.P_Users.ID == SelectedId
                                        select cust)
                                        .AsEnumerable()
                                        .Select(cust => new
                                        {
                                            cust.ID,
                                            cust.SingleDate,
                                            Day =  culture.DateTimeFormat.GetDayName(((DayOfWeek)Enum.Parse(typeof(DayOfWeek), cust.Day))),
                                            cust.TimeIn,
                                            cust.TimeOut,
                                            cust.LengthOfInside,
                                            cust.LengthOfOutside
                                        });

                GridMonthlyReport.ItemsSource = queryAllCustomers.ToList();
                GridMonthlyReport.Items.Refresh();
            }
            else
            {
                MessageBox.Show((string)Application.Current.Resources["msb_userDontHaveTime"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GridMonthlyReport.ItemsSource = null;
            }

            // get your 'Uploaded' folder
            var dir = new DirectoryInfo(Singelton.PathToPhoto);
            var queryPhoto = from cust in db.C_Users
                             where cust.ID == SelectedId
                             select new { cust.Photo };
            var Names = db.Database
                .SqlQuery<M_Names>(new SqlGetUserName().SQl_GetUserFirstName, (object)SelectedId)
                .SingleOrDefault();
            var firstName = Names.NameFirst;
            var lastName = Names.NameLast;
            var z = queryPhoto.ToList();
            NameUser.Text = firstName + " " + lastName;
            string str = dir.FullName + "/" + z[0].Photo;

            try
            {
                string NameImage = z[0].Photo;
                string[] words = NameImage.Split('_');
                int indexdot = words[2].IndexOf('.');
                NameUser.Text = words[1] + " " + words[2].Remove(indexdot);
                imageReport.ImageSource = ImageLoaderHelper.GetImageFromFolder(str);

            }
            catch (Exception)
            {
                imageReport.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Image/ProfileIcon.png"));
                string message = (string)Application.Current.Resources["msb_userDontHavePhoto"];
                Dispatcher.BeginInvoke(new Action (() => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)));
            }

            _isSearch = false;
            db.Dispose();
        }
        private void GridMonthlyReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ClickFindUser(object sender, RoutedEventArgs e)
        {
            FindUserPage p = new FindUserPage();
            this.NavigationService.Navigate(p);
        }
        private void ClickAddUser(object sender, RoutedEventArgs e)
        {
            AddOrUpdateUserPage p = new AddOrUpdateUserPage();
            this.NavigationService.Navigate(p);
        }
        private void ClickPhotoUser(object sender, MouseButtonEventArgs e)
        {
            dynamic s = GridBasicInformation.CurrentCell.Item;

            if (s == DependencyProperty.UnsetValue)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_userNotSelected"], "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                int id = s.ID;
                AboutUserPage p = new AboutUserPage(id);
                this.NavigationService.Navigate(p);
            }
        }
        private void buttonAllTime_Click(object sender, RoutedEventArgs e)
        {
            HideGrids();
            AllDatePage p;

            if (SelectedId == null)
            {
                p = new AllDatePage();
            }
            else
            {
                p = new AllDatePage(SelectedId);
            }

            GridAllTime.NavigationService.Navigate(p);
            GridAllTime.Visibility = Visibility.Visible;
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["fp_lbi_HourseWork"]);
        }
        private void GridBasicInformation_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Start")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm";
            }
            if (e.Column.Header.ToString() == "End")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm";
            }
            if (e.Column.Header.ToString() == "Valid")
            {
                e.Column.ClipboardContentBinding.StringFormat = "00:mm";
            }
        }
        private void SelectedBasicInformation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > "+Application.Current.Resources["fp_lbi_MainInformation"]);
            HideGrids();
            ViewBasicInformation.Visibility = Visibility.Visible;
        }
        private void SelectedMonthlyReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["fp_lbi_MonthlyReport"]);
            HideGrids();
            ViewMonthlyReport.Visibility = Visibility.Visible;
        }
        private void SelectedDepartamentReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["fp_lbi_DepReport"]);
            HideGrids();
            ViewDepartamentReport.Visibility = Visibility.Visible;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            NameUser.Text = Application.Current.Resources["u_l_FirstName"] +
               " " + Application.Current.Resources["u_l_SecondName"];
            Singelton.MainWindow.ChangeStringAddres(Title);
        }
        private void GridMonthlyReport_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "SingleDate")
            {
                e.Column.ClipboardContentBinding.StringFormat = "d/M/yyyy";
            }
            if (e.Column.Header.ToString() == "TimeIn")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm:ss";
            }
            if (e.Column.Header.ToString() == "TimeOut")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm:ss";
            }
            if (e.Column.Header.ToString() == "LengthOfInside")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm:ss";
            }
            if (e.Column.Header.ToString() == "LengthOfOutside")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm:ss";
            }

        }
        private void UnselecctButton_Click(object sender, RoutedEventArgs e)
        {
            GridBasicInformation.SelectedIndex = -1;
            SelectedId = null;
            NameUser.Text = Application.Current.Resources["u_l_FirstName"] +
              " " + Application.Current.Resources["u_l_SecondName"];
        }
        private void GenerationExcel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HideGrids();
            SVGenerationExcel.Visibility = Visibility.Visible;
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["fp_lbi_Excel"]);
            GenerationAndSendingExcel p = new GenerationAndSendingExcel();
            p.SetGrids(GridBasicInformation, GridMonthlyReport, GridDepartamentReport);
            SVGenerationExcel.Navigate(p);
        }
        private void GridBasicInformation_AutoGeneratedColumns(object sender, EventArgs e)
        {
            GridBasicInformation.Columns[1].Header = Application.Current.Resources["u_l_FirstName"];
            GridBasicInformation.Columns[2].Header = Application.Current.Resources["u_l_SecondName"];
            GridBasicInformation.Columns[3].Header = Application.Current.Resources["u_l_ThirdName"];
            GridBasicInformation.Columns[4].Header = Application.Current.Resources["u_l_DepartmentName"];
            GridBasicInformation.Columns[5].Header = Application.Current.Resources["dgc_stat"];
            GridBasicInformation.Columns[6].Header = Application.Current.Resources["dgc_end"];
            GridBasicInformation.Columns[7].Header = Application.Current.Resources["dgc_valid"];
            GridBasicInformation.Columns[8].Header = Application.Current.Resources["dgc_dinner"];
            GridBasicInformation.Columns[8].ClipboardContentBinding.StringFormat = "HH:mm";
            if (SelectedId != null)
            {

                foreach (var item in GridBasicInformation.Items)
                {
                    dynamic temp = item;

                    if (temp.ID == SelectedId)
                    {
                        _isSearch = true;
                        GridBasicInformation.SelectedItem = item;
                    }
                }
            }
        }
        private void GridMonthlyReport_AutoGeneratedColumns(object sender, EventArgs e)
        {
            GridMonthlyReport.Columns[1].Header = Application.Current.Resources["dgc_date"];
            GridMonthlyReport.Columns[2].Header = Application.Current.Resources["dgc_day"];
            GridMonthlyReport.Columns[3].Header = Application.Current.Resources["dgc_timeIn"];
            GridMonthlyReport.Columns[4].Header = Application.Current.Resources["dgc_timeOut"];
            GridMonthlyReport.Columns[5].Header = Application.Current.Resources["dgc_lenghtInside"];
            GridMonthlyReport.Columns[6].Header = Application.Current.Resources["dgc_leanghtOuside"];
        }
        private void GridDepartamentReport_AutoGeneratedColumns(object sender, EventArgs e)
        {
            GridDepartamentReport.Columns[0].Header = Application.Current.Resources["dgc_date"];
            GridDepartamentReport.Columns[1].Header = Application.Current.Resources["u_l_DepartmentName"];
            GridDepartamentReport.Columns[2].Header = Application.Current.Resources["dgc_count"];
            GridDepartamentReport.Columns[3].Header = Application.Current.Resources["dgc_countInSide"];
            GridDepartamentReport.Columns[4].Header = Application.Current.Resources["dgc_countOutSide"];
        }

    }
}
