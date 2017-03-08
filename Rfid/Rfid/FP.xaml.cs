using Rfid.Context;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for FP.xaml
    /// </summary>
    public partial class FP : Page
    {
        int SelectedId;
        public FP()
        {
            InitializeComponent();

            RfidContext db = new RfidContext();

            var queryAllCustomers = from cust in db.C_Names
                                    where cust.P_Users.IsUser == true
                                    select new
                                    {
                                        cust.P_Users.ID,
                                        cust.NameFirst,
                                        cust.NameLast,
                                        cust.NameThird,
                                        cust.P_Users.P_Departments.Name,
                                        cust.P_Users.P_InOutValidTimes.Start,
                                        cust.P_Users.P_InOutValidTimes.And,
                                        cust.P_Users.P_InOutValidTimes.Valid
                                    };
            GridBasicInformation.EnableColumnVirtualization = true;
            GridBasicInformation.ItemsSource = queryAllCustomers.ToList();
            GridBasicInformation.EnableColumnVirtualization = true;
            GridBasicInformation.Items.Refresh();
            var a = GridBasicInformation.CommandBindings;
            //this.GridMonthlyReport.Columns[3].ClipboardContentBinding.StringFormat = "HH:mm";

            var queryAllDepartamentsNames = from cust in db.C_Departments
                                            select new { cust.Name }
                                       ;

            List<DepartamentsData> listDepInfo = new List<DepartamentsData>();
            foreach (var depName in queryAllDepartamentsNames.ToList())
            {
                DepartamentsData temp = getDepartamentClientData(depName.Name);
                listDepInfo.Add(temp);
            }

            GridDepartamentReport.ItemsSource = listDepInfo;


            this.GridDepartamentReport.Items.Refresh();


            var d = GridDepartamentReport.Columns;

            //GridDepartamentReport.Columns[0].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
        }

        //private IQueryable ToTimeSpanFromLong(IQueryable collection)
        //{
        //    IQueryable res;

        //    foreach (var temp in collection)
        //    {

        //    }


        //    return res;
        //}

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

        private void GridBasicInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic s = GridBasicInformation.CurrentCell.Item;
            SelectedId = s.ID;

            RfidContext db = new RfidContext();

            var queryAllCustomers = from cust in db.C_UserTime
                                    where cust.P_Users.ID == SelectedId
                                    select new
                                    {
                                        cust.ID,
                                        cust.SingleDate,
                                        cust.Day,
                                        cust.TimeIn,
                                        cust.TimeOut,
                                        cust.LengthOfInside,
                                        cust.LengthOfOutside
                                    };

            GridMonthlyReport.ItemsSource = queryAllCustomers.ToList();

            //this.GridBasicInformation.Columns[5].ClipboardContentBinding.StringFormat = "HH:mm";
            //this.GridBasicInformation.Columns[6].ClipboardContentBinding.StringFormat = "HH:mm";
            //this.GridBasicInformation.Columns[7].ClipboardContentBinding.StringFormat = "HH:mm";

            this.GridMonthlyReport.Columns[3].ClipboardContentBinding.StringFormat = "HH:mm";
            this.GridMonthlyReport.Columns[4].ClipboardContentBinding.StringFormat = "HH:mm";
            this.GridMonthlyReport.Columns[5].ClipboardContentBinding.StringFormat = "HH:mm";
            this.GridMonthlyReport.Columns[6].ClipboardContentBinding.StringFormat = "HH:mm";

            GridMonthlyReport.Items.Refresh();

            var applicationPath = Directory.GetCurrentDirectory();

            // get your 'Uploaded' folder
            var dir = new DirectoryInfo(System.IO.Path.Combine(applicationPath, "Photos"));


            var queryPhoto = from cust in db.C_Users
                             where cust.ID == SelectedId
                             select new { cust.Photo };

            var z = queryPhoto.ToList();

            string str = dir.FullName + "/" + z[0].Photo;

            imageReport.Source = new BitmapImage(new Uri(str));
        }

        private void GridMonthlyReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClickFindUser(object sender, RoutedEventArgs e)
        {
            UserInfo_Find p = new UserInfo_Find();
            this.NavigationService.Navigate(p);
        }

        private void ClickAddUser(object sender, RoutedEventArgs e)
        {
            UserInfo_Add p = new UserInfo_Add();
            this.NavigationService.Navigate(p);
        }


        private void ClickPhotoUser(object sender, MouseButtonEventArgs e)
        {
            dynamic s = GridBasicInformation.CurrentCell.Item;
            int id = s.ID;

            AboutUser p = new AboutUser(id);
            this.NavigationService.Navigate(p);

            //AboutUserWindow aboutUserWind = new AboutUserWindow(id);
            //aboutUserWind.Show();
        }

        private void buttonAllTime_Click(object sender, RoutedEventArgs e)
        {

            AllDatePage p = new AllDatePage(SelectedId);
            this.NavigationService.Navigate(p);
        }

        private void ClickAddRfid(object sender, RoutedEventArgs e)
        {
            WriteById();
        }
        private void WriteById()
        {
            try
            {
                RfidContext db = new RfidContext();

                #region User information

                M_Users user = new M_Users();

                // HelperPortDataReceived portDataReceived = new HelperPortDataReceived();
                // long s = portDataReceived.ParseInData;
                GetRFIDIdWindow getId = new GetRFIDIdWindow();

                getId.ShowDialog();

                long s = Convert.ToInt64(getId.TBGetRfidId.Text);

                var dsd = db.C_Rfids.Where(x => x.RfidID == s).Single();

                //user = db.C_Users.Where(z => z.P_Rfids.Contains(db.C_Rfids.Where(x => x.RfidID == s).Single()) ).Single();
                #endregion

                if (dsd.P_Users.isInside == false)
                {
                    TimeSpan? lengthOfInside = new TimeSpan();
                    if (dsd.P_Users.P_UserTime.Count != 0)
                    {
                        lengthOfInside = DateTime.Now - dsd.P_Users.P_UserTime.Last().TimeOut;
                        DateTime date = DateTime.Today.Add(lengthOfInside.Value);

                        DateTime? dtOutsige = DateTime.Today.Add(lengthOfInside.Value);

                        dsd.P_Users.P_UserTime.Last().LengthOfOutside = dtOutsige;
                    }
                    dsd.P_Users.P_UserTime.Add(new M_UserTime { SingleDate = DateTime.Now.Date });
                    dsd.P_Users.P_UserTime.Last().Day = DateTime.Now.DayOfWeek.ToString();
                    dsd.P_Users.P_UserTime.Last().TimeIn = DateTime.Now;

                    dsd.P_Users.isInside = true;
                }
                else
                {
                    dsd.P_Users.P_UserTime.Last().TimeOut = DateTime.Now;

                    TimeSpan? lengthOfInside = dsd.P_Users.P_UserTime.Last().TimeOut - dsd.P_Users.P_UserTime.Last().TimeIn;

                    DateTime? dtInsige = DateTime.Today.Add(lengthOfInside.Value);


                    dsd.P_Users.P_UserTime.Last().LengthOfInside = dtInsige;


                    dsd.P_Users.isInside = false;
                }

                db.SaveChanges();

            }
            catch
            {
                throw;
            }
        }

        private void GridBasicInformation_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Start")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm";
            }
            if (e.Column.Header.ToString() == "And")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm";
            }
            if (e.Column.Header.ToString() == "Valid")
            {
                e.Column.ClipboardContentBinding.StringFormat = "HH:mm";
            }
        }
    }
}