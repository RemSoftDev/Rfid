using Microsoft.Win32;
using Rfid.Context;
using Rfid.Helpers;
using Rfid.Models;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> pnoneNumbersUser = new List<string>();
        List<string> pnoneNumbersContactMan = new List<string>();
        List<string> pnoneNumbersDirector = new List<string>();
        List<M_Users> contactMan = new List<M_Users>();

        HelperUser HU = new HelperUser();
        RfidContext db = new RfidContext();

        public MainWindow()
        {
            try
            {
                db.C_Users.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
         
            InitializeComponent();

            UserInfo_Main p = new UserInfo_Main();
            MainFrame.NavigationService.Navigate(p);
        }

        //private void Button_UploadImage(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog op = new OpenFileDialog();
        //    op.Title = "Select a photo";
        //    op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        //                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        //                "Portable Network Graphic (*.png)|*.png";

        //    if (op.ShowDialog() == true)
        //    {
        //        imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
        //    }
        //}

        //private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var picker = sender as DatePicker;
        //    DateTime? date = picker.SelectedDate;

        //    if (date == null)
        //    {
        //        this.Title = "No date";
        //    }
        //    else
        //    {
        //        this.Title = date.Value.ToShortDateString();
        //    }
        //}

        //private void Button_AddUserPhone_Click(object sender, RoutedEventArgs e)
        //{
        //    var grid = Button_AddUserPhone.Parent as Grid;

        //    HU.AddPhone(grid, TB_UserPhone, 2, 3, pnoneNumbersUser);

        //}

        //private void Button_AddContactManPhone_Click(object sender, RoutedEventArgs e)
        //{
        //    var grid = Button_AddContacnManPhone.Parent as Grid;

        //    HU.AddPhone(grid, TB_ContacnManPhone, 2, 3, pnoneNumbersContactMan);
        //}

        //private void Button_AddContactMan_Click(object sender, RoutedEventArgs e)
        //{
        //    var grid = Button_AddContacnMan.Parent as Grid;

        //    HU.AddContactMan(grid, TB_FN, TB_SN, TB_TN, TB_ContacnManPhone, 5, contactMan, pnoneNumbersContactMan);
        //    HU.ClearPhones(Button_AddContacnManPhone.Parent as Grid, 2, 3, TB_ContacnManPhone);
        //}

        //private void Button_AddDirectPhone_Click(object sender, RoutedEventArgs e)
        //{
        //    var grid = Button_AddDirectorPhone.Parent as Grid;

        //    HU.AddPhone(grid, TB_DirectorPhone, 1, 7, pnoneNumbersDirector);
        //}

        //private void Button_Save_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string imagepath = ((BitmapImage)imgPhoto.Source).UriSource.AbsolutePath;
        //        var imageFile = new System.IO.FileInfo(imagepath);

        //        if (imageFile.Exists)// check image file exist
        //        {
        //            // get your application folder
        //            var applicationPath = Directory.GetCurrentDirectory();

        //            // get your 'Uploaded' folder
        //            var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "uploaded"));
        //            if (!dir.Exists)
        //                dir.Create();
        //            // Copy file to your folder
        //            //imageFile.CopyTo(System.IO.Path.Combine(dir.FullName, string.Format("{0}_{1}",
        //            //    FirstName.Text.Replace(" ", String.Empty),
        //            //    LastName.Text.Replace(" ", String.Empty))));
        //        }

        //        // Contact men
        //        M_Names contactManNames = new M_Names { NameFirst = TB_UserFN.Text, NameLast = TB_UserSN.Text, NameThird = TB_UserTN.Text };
        //        List<M_Names> contactManNamesList = new List<M_Names>();
        //        contactManNamesList.Add(contactManNames);

        //        // Address
        //        string address = TB_UsesrAddr.Text;
        //        // Birthday
        //        DateTime? birth = DP_UserBithday.SelectedDate;

        //        // Phones
        //        List<M_Phones> userPhones = new List<M_Phones>();

        //        foreach (string item in pnoneNumbersUser)
        //        {
        //            userPhones.Add(new M_Phones { PhoneNumber = item });
        //        }

        //        // Department
        //        M_Names departmentName = new M_Names { NameFirst = TB_DepFN.Text, NameLast = TB_DepSN.Text, NameThird = TB_DepTN.Text };
        //        List<M_Names> departmentNames = new List<M_Names>();
        //        departmentNames.Add(departmentName);

        //        List<M_Phones> directorPhones = new List<M_Phones>();

        //        foreach (string item in pnoneNumbersDirector)
        //        {
        //            directorPhones.Add(new M_Phones { PhoneNumber = item });
        //        }

        //        M_Departments department = new M_Departments
        //        {
        //            CodeFull = TB_DepCode.Text,
        //            Name = TB_DepName.Text,
        //            DepartmentDirectorName = departmentNames,
        //            DepartmentDirectorPhone = directorPhones
        //        };

        //        // User
        //        M_Users user = new M_Users();
        //        user.P_Names = contactManNamesList;
        //        user.Address = address;
        //        user.D_Birth = birth;
        //        user.P_ManForContact = contactMan;
        //        user.P_Phones = userPhones;
        //        user.P_Departments = department;

        //        db.C_Users.Add(user);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        //private void Button_Back_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService navService = NavigationService.GetNavigationService(this);
        //    //navService.Navigate(new Uri("/UserInfo.xaml", UriKind.RelativeOrAbsolute));
        //    UserInfo p = new UserInfo();
        //    navService.Navigate(p);
        //}
    }
}
