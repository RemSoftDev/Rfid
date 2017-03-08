using Microsoft.Win32;
using Rfid.Context;
using Rfid.Helpers;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for AddUserInfo.xaml
    /// </summary>
    public partial class UserInfo_Add : Page
    {
        List<string> pnoneNumbersUser = new List<string>();
        List<string> pnoneNumbersContactMan = new List<string>();
        List<string> pnoneNumbersDirector = new List<string>();
        List<M_Users> contactMan = new List<M_Users>();

        HelperUser HU = new HelperUser();
        RfidContext db = new RfidContext();

        public UserInfo_Add()
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

            if (UserInfo_Main.checkedPort.Length == 0)
            {
                AddRfid.IsEnabled = false;
            }
        }

        public UserInfo_Add(Guid userId)
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
        }

        private void Button_UploadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a photo";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;

            if (date == null)
            {
                this.Title = "No date";
            }
            else
            {
                this.Title = date.Value.ToShortDateString();
            }
        }

        private void Button_AddUserPhone_Click(object sender, RoutedEventArgs e)
        {
            var grid = Button_AddUserPhone.Parent as Grid;

            HU.AddPhone(grid, TB_UserPhone, 2, 3, pnoneNumbersUser);
        }

        private void Button_AddContactManPhone_Click(object sender, RoutedEventArgs e)
        {
            var grid = Button_AddContacnManPhone.Parent as Grid;

            HU.AddPhone(grid, TB_ContacnManPhone, 2, 3, pnoneNumbersContactMan);
        }

        private void Button_AddContactMan_Click(object sender, RoutedEventArgs e)
        {
            var grid = Button_AddContacnMan.Parent as Grid;

            HU.AddContactMan(grid, TB_FN, TB_SN, TB_TN, TB_ContacnManPhone, 5, contactMan, pnoneNumbersContactMan);
            HU.ClearPhones(Button_AddContacnManPhone.Parent as Grid, 2, 3, TB_ContacnManPhone);
        }

        private void Button_AddDirectPhone_Click(object sender, RoutedEventArgs e)
        {
            var grid = Button_AddDirectorPhone.Parent as Grid;

            HU.AddPhone(grid, TB_DirectorPhone, 1, 7, pnoneNumbersDirector);
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Label> listLableContact = new List<Label>();

                listLableContact.Add(HelperUser.FindChild<Label>(Application.Current.MainWindow, "L_ContactPhone3"));
                listLableContact.Add(HelperUser.FindChild<Label>(Application.Current.MainWindow, "L_ContactPhone3"));
                // Contact men
                M_Names contactManNames = new M_Names
                {
                    NameFirst = TB_FN.Text,
                    NameLast = TB_SN.Text,
                    NameThird = TB_TN.Text
                };

                M_Phones contacPhones = new M_Phones
                {
                    PhoneNumber = TB_ContacnManPhone.Text + "," + listLableContact[0].Content.ToString() + "," + listLableContact[1].Content.ToString()
                };

                List<M_Names> contactManNamesList = new List<M_Names>();
                contactManNamesList.Add(contactManNames);

                List<M_Phones> phonesContactMan = new List<M_Phones>();

                phonesContactMan.Add(contacPhones);

                List<M_Users> listUser = new List<M_Users>();

                M_Users contactUserTextBox = new M_Users
                {
                    P_Names = contactManNamesList,
                    Address = TB_UsesrAddr.Text,
                    P_Phones = phonesContactMan
                };

                listUser.Add(contactUserTextBox);

                List<Label> listLable = new List<Label>();

                listLable.Add(HelperUser.FindChild<Label>(Application.Current.MainWindow, "L_Contact1"));
                listLable.Add(HelperUser.FindChild<Label>(Application.Current.MainWindow, "L_Contact2"));

                foreach (Label tempLab in listLable)
                {
                    if (tempLab != null)
                    {
                        List<M_Names> contactManNamesListLabel = new List<M_Names>();
                        List<M_Phones> phonesContactManLabel = new List<M_Phones>();

                        string contentSecond = tempLab.Content.ToString();
                        string[] split;
                        split = contentSecond.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                        M_Names contactManNamesAdd = new M_Names
                        {
                            NameFirst = split[0],
                            NameLast = split[1],
                            NameThird = split[2]
                        };

                        contactManNamesListLabel.Add(contactManNamesAdd);

                        for (int i = 3; i < split.Length; i++)
                        {
                            M_Phones phone = new M_Phones() { PhoneNumber = split[i] };
                            phonesContactManLabel.Add(phone);
                        }

                        M_Users contactUser = new M_Users
                        {
                            P_Names = contactManNamesListLabel,
                            Address = TB_UsesrAddr.Text,
                            P_Phones = phonesContactManLabel
                        };

                        listUser.Add(contactUser);
                    }
                }

                // Address
                string address = TB_UsesrAddr.Text;
                // Birthday
                DateTime? birth = DP_UserBithday.SelectedDate;

                // Phones
                List<M_Phones> userPhones = new List<M_Phones>();

                foreach (string item in pnoneNumbersUser)
                {
                    userPhones.Add(new M_Phones { PhoneNumber = item });
                }

                // Department
                M_Names departmentName = new M_Names { NameFirst = TB_DepFN.Text, NameLast = TB_DepSN.Text, NameThird = TB_DepTN.Text };
                List<M_Names> departmentNames = new List<M_Names>();
                departmentNames.Add(departmentName);

                List<M_Phones> directorPhones = new List<M_Phones>();

                foreach (string item in pnoneNumbersDirector)
                {
                    directorPhones.Add(new M_Phones { PhoneNumber = item });
                }

                M_Departments department = new M_Departments
                {
                    CodeFull = TB_DepCode.Text,
                    Name = TB_DepName.Text,
                    DepartmentDirectorName = departmentNames,
                    DepartmentDirectorPhone = directorPhones
                };

                // User

                M_Names contactManNamess = new M_Names { NameFirst = TB_UserFN.Text, NameLast = TB_UserSN.Text, NameThird = TB_UserTN.Text };

                List<M_Names> pName = new List<M_Names>();
                pName.Add(contactManNamess);

                List<M_Users> contactMan = new List<M_Users>();

                // contactMan.Add();

                M_Users user = new M_Users();
                user.P_Names = pName;
                user.Address = address;
                user.D_Birth = birth;
                user.P_ManForContact = listUser;
                user.P_Phones = userPhones;


                bool isNoFind = true;
                foreach (var dep in db.C_Departments)
                {
                    if (dep.Name == department.Name && dep.CodeFull == department.CodeFull)
                    {
                        dep.P_Users.Add(user);
                        isNoFind = false;

                        break;
                    }
                }
                if (isNoFind)
                {
                    user.P_Departments = department;
                }

                user.IsUser = true;

                M_Rfids rFids = new M_Rfids();

                rFids.RfidID = Convert.ToInt64(ShowRfid.Text);
                rFids.Date = DateTime.Now;

                user.P_Rfids = new List<M_Rfids>();
                user.P_Rfids.Add(rFids);

                // Image
                string imagepath = ((BitmapImage)imgPhoto.Source).UriSource.AbsolutePath;

                var imageFile = new FileInfo(imagepath);

                string imageName = string.Empty;

                if (imageFile.Exists)// check image file exist
                {
                    imageName = string.Format("{0}_{1}_{2}.{3}",
                        user.ID.ToString(),
                        TB_UserFN.Text.Replace(" ", string.Empty),
                        TB_UserSN.Text.Replace(" ", string.Empty),
                        imageFile.ToString().Split('.').Last()
                        );

                    // get your application folder
                    var applicationPath = Directory.GetCurrentDirectory();

                    // get your 'Uploaded' folder
                    var dir = new DirectoryInfo(System.IO.Path.Combine(applicationPath, "Photos"));

                    if (!dir.Exists)
                        dir.Create();

                    // Copy file to your folder
                    imageFile.CopyTo(System.IO.Path.Combine(dir.FullName, imageName));
                }

                user.Photo = imageName;
                DateTime start = new DateTime(2000, 01, 01);
                DateTime and = new DateTime(2000, 01, 01);
                DateTime valid = new DateTime(2000, 01, 01);

                TimeSpan start1 = TimeSpan.Parse(TB_Start.Text);
                TimeSpan and1 = TimeSpan.Parse(TB_And.Text);
                TimeSpan valid1 = TimeSpan.Parse(TB_Valid.Text);

                start+=(start1);
                and+=(and1);
                valid+=(valid1);

                M_InOutValidTimes time = new M_InOutValidTimes
                {
                    Start = start,
                    And = and,
                    Valid = valid
                };

                user.P_InOutValidTimes = time;

                db.C_Users.Add(user);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            UserInfo_Main p = new UserInfo_Main();
            this.NavigationService.Navigate(p);
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            UserInfo_Main p = new UserInfo_Main();
            this.NavigationService.Navigate(p);
        }

        private void Click_AddRfid(object sender, RoutedEventArgs e)
        {
            HelperPortDataReceived portDataReceived = new HelperPortDataReceived();
            ShowRfid.Text = portDataReceived.ParseInData.ToString();
        }
    }
}
