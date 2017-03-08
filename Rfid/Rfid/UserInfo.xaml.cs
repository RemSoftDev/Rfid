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
using Rfid.Models;
using Rfid.Helpers;
using System.IO;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Page
    {
        static int Id = 0;
        public UserInfo()
        {
            InitializeComponent();


            //RfidContext db = new RfidContext();

            //#region User information

            //M_Users user = new M_Users();

            //// HelperPortDataReceived portDataReceived = new HelperPortDataReceived();
            //// long s = portDataReceived.ParseInData;
            //long s = 1;
            //var dsd = db.C_Rfids.Where(x => x.RfidID == s).Single();

            ////user = db.C_Users.Where(z => z.P_Rfids.Contains(db.C_Rfids.Where(x => x.RfidID == s).Single()) ).Single();
            //#endregion

            //if (dsd.P_Users.isInside == false)
            //{
            //    TimeSpan? lengthOfInside = new TimeSpan();
            //    if (dsd.P_Users.P_UserTime.Count != 0)
            //    {
            //        lengthOfInside = DateTime.Now - dsd.P_Users.P_UserTime.Last().TimeOut;
            //        DateTime date = DateTime.Today.Add(lengthOfInside.Value);

            //        DateTime? dtOutsige = DateTime.Today.Add(lengthOfInside.Value);

            //        dsd.P_Users.P_UserTime.Last().LengthOfOutside = dtOutsige;
            //    }
            //    dsd.P_Users.P_UserTime.Add(new M_UserTime { SingleDate = DateTime.Now.Date });
            //    dsd.P_Users.P_UserTime.Last().Day = DateTime.Now.DayOfWeek.ToString();
            //    dsd.P_Users.P_UserTime.Last().TimeIn = DateTime.Now;

            //    dsd.P_Users.isInside = true;
            //}
            //else
            //{
            //    dsd.P_Users.P_UserTime.Last().TimeOut = DateTime.Now;

            //    TimeSpan? lengthOfInside = dsd.P_Users.P_UserTime.Last().TimeOut - dsd.P_Users.P_UserTime.Last().TimeIn;

            //    DateTime? dtInsige = DateTime.Today.Add(lengthOfInside.Value);


            //    dsd.P_Users.P_UserTime.Last().LengthOfInside = dtInsige;


            //    dsd.P_Users.isInside = false;
            //}

            //db.SaveChanges();



          //  GetUserInformation(dsd.P_Users.ID);
        }

        public UserInfo(int id)
        {

            GetUserInformation(id);
        }

        public void GetUserInformation(int Id)
        {
            RfidContext db = new RfidContext();

            #region User information

            M_Users user = new M_Users();

            List<M_Names> names = new List<M_Names>();

            List<M_Rfids> prfids = new List<M_Rfids>();

            user = db.C_Users.Where(z => z.ID == Id).Single();

            names = user.P_Names.ToList();

            prfids = user.P_Rfids.ToList();

            TB_UserFN.Text = names[0].NameFirst;
            TB_UserSN.Text = names[0].NameLast;
            TB_UserTN.Text = names[0].NameThird;
            TB_UsesrAddr.Text = user.Address;
            try
            {
                TB_Birthday.Text = user.D_Birth.Value.ToShortDateString();
            }
            catch (Exception ex)
            {

            }
            TB_Rfid.Text = prfids[0].RfidID.ToString();
            List<M_Phones> userPhones = new List<M_Phones>();

            userPhones = user.P_Phones.ToList();

            TB_UserPhone.Text = string.Empty;

            var gridUserPhones = TB_UserSN.Parent as Grid;
            int nubbeOfRow = 0;
            foreach (M_Phones phone in userPhones)
            {
                Label labelUserPhone = new Label();
                labelUserPhone.Content = phone.PhoneNumber;

                Grid.SetColumn(labelUserPhone, 2);
                Grid.SetRow(labelUserPhone, 3 + nubbeOfRow);
                nubbeOfRow++;
                gridUserPhones.Children.Add(labelUserPhone);
            }
            #endregion


            #region Department information

            List<M_Names> departmentsName = new List<M_Names>();
            departmentsName = user.P_Departments.DepartmentDirectorName.ToList();

            List<M_Phones> departmentsPhones = new List<M_Phones>();
            departmentsPhones = user.P_Departments.DepartmentDirectorPhone.ToList();

            TB_DepName.Text = user.P_Departments.Name;
            TB_DepCode.Text = user.P_Departments.CodeFull;

            TB_DepFN.Text = departmentsName[0].NameFirst;
            TB_DepSN.Text = departmentsName[0].NameLast;
            TB_DepTN.Text = departmentsName[0].NameThird;

            var applicationPath = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(System.IO.Path.Combine(applicationPath, "Photos"));

            string str = dir.FullName + "/" + user.Photo.ToString();

            imgPhoto.Source = new BitmapImage(new Uri(str));

            TB_DirectorPhone.Text = string.Empty;

            var gridDepartmentsPhones = TB_DepName.Parent as Grid;
            nubbeOfRow = 0;
            foreach (M_Phones phone in departmentsPhones)
            {
                Label labelUserPhone = new Label();
                labelUserPhone.Content = phone.PhoneNumber;

                Grid.SetColumn(labelUserPhone, 0);
                Grid.SetRow(labelUserPhone, 6 + nubbeOfRow);
                nubbeOfRow++;
                gridDepartmentsPhones.Children.Add(labelUserPhone);
            }

            #endregion

            #region Add man
            List<M_Users> contactUsers = new List<M_Users>();
            contactUsers = user.P_ManForContact.ToList();
            List<M_Names> contactNames = new List<M_Names>();
            List<M_Phones> contactPhones = new List<M_Phones>();

            nubbeOfRow = 0;
            TB_FN.Text = string.Empty;
            foreach (M_Users cUser in contactUsers)
            {
                contactNames = cUser.P_Names.ToList();
                contactPhones = cUser.P_Phones.ToList();

                var grid = L_FN.Parent as Grid;

                Label labelUserName = new Label();
                labelUserName.Content = contactNames[0].NameFirst + " " +
                                        contactNames[0].NameLast + " " +
                                        contactNames[0].NameThird + " ";

                Grid.SetColumn(labelUserName, 0);
                Grid.SetRow(labelUserName, 5 + nubbeOfRow);

                grid.Children.Add(labelUserName);

                foreach (M_Phones cPhones in contactPhones)
                {
                    Label labelUserPhone = new Label();
                    labelUserPhone.Content += cPhones.PhoneNumber;

                    Grid.SetColumn(labelUserPhone, 1);
                    Grid.SetRow(labelUserPhone, 5 + nubbeOfRow);

                    grid.Children.Add(labelUserPhone);
                }
                nubbeOfRow++;

            }

            #endregion
        }

        private string GetApplicationFolder()
        {
            throw new NotImplementedException();
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            UserInfo_Add p = new UserInfo_Add(new Guid());
            this.NavigationService.Navigate(p);
        }

        private void Button_RfidsNumbersShow(object sender, RoutedEventArgs e)
        {
            UserInfo_RfidsNumbers p = new UserInfo_RfidsNumbers();
            this.NavigationService.Navigate(p);
        }
    }
}
