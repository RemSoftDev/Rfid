using Microsoft.Win32;
using Rfid.AutoCompliteTextBox;
using Rfid.Context;
using Rfid.Helpers;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Metro = MahApps.Metro.Controls;

namespace Rfid
{
    public partial class AddOrUpdateUserPage : Page
    {
        List<string> pnoneNumbersUser = new List<string>();
        List<string> pnoneNumbersContactMan = new List<string>();
        List<string> pnoneNumbersDirector = new List<string>();
        List<M_Users> contactMan = new List<M_Users>();
        UserHelper HU = new UserHelper();
        RfidContext db = new RfidContext();
        private int? iD;
        private string imagePath;
        public AddOrUpdateUserPage()
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_UpdateImage"]);

            try
            {
                db.C_Users.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            InitializeComponent();

            if (StartUpPage.checkedPort.Length == 0)
            {
                AddRfid.IsEnabled = false;
            }
        }
        public AddOrUpdateUserPage(Guid userId)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_UpdateImage"]);

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
        public AddOrUpdateUserPage(int id)
        {
            InitializeComponent();
            iD = id;
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_UpdateImage"]);

            CurrentUser = (from x in db.C_Users
                           where x.ID == id
                           select x).Single();
        
            var dir = new DirectoryInfo(Singelton.PathToPhoto);
            string str = dir.FullName + "/" + CurrentUser.Photo;

            try
            {
                imgPhoto.Source = ImageLoaderHelper.GetImageFromFolder(str);
                IsImageExist = false;
            }
            catch (Exception)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_userDontHavePhoto"],"Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            //UserInformationSelection
            M_Names NameCurrentUser = CurrentUser.P_Names.Single();
            TB_UserFN.Text = NameCurrentUser.NameFirst;
            TB_UserSN.Text = NameCurrentUser.NameLast;
            TB_UserTN.Text = NameCurrentUser.NameThird;
            TB_UsesrAddr.Text = CurrentUser.Address;
            DP_UserBithday.SelectedDate = CurrentUser.D_Birth;
            List<M_Phones> PhoneCurrentUser = CurrentUser.P_Phones.ToList();

            Action<TextBox, TextBox, TextBox, List<M_Phones>> setPhones = (tb1, tb2, tb3, listPhones) =>
            {
                if (listPhones.Count > 0)
                {
                    tb1.Text = listPhones[0].PhoneNumber;
                }
                if (listPhones.Count > 1)
                {
                    tb2.Text = listPhones[1].PhoneNumber;
                }
                if (listPhones.Count > 2)
                {
                    tb3.Text = listPhones[2].PhoneNumber;
                }
            };
            setPhones(PhoneNumber1, PhoneNumber2, PhoneNumber3, PhoneCurrentUser);

            //contactMan
            Action<TextBox, TextBox, TextBox, TextBox, TextBox, TextBox, M_Users> setOneContactMan =
                (fn, sn, tn, tb1, tb2, tb3, cmUser) =>
                {
                    M_Names cmUserName = cmUser.P_Names.Single();
                    fn.Text = cmUserName.NameFirst;
                    sn.Text = cmUserName.NameLast;
                    tn.Text = cmUserName.NameThird;
                    setPhones(tb1, tb2, tb3, cmUser.P_Phones.ToList());
                };

            Action<List<M_Users>> setAllContactMans =
                (cmUsers) =>
                {
                    if (cmUsers.Count > 0)
                    {
                        CMFirstShow.Visibility = Visibility.Visible;
                        setOneContactMan(CMFirstName1, CMSecondName1, CMThirdName1,
                            CMPhone11, CMPhone12, CMPhone13, cmUsers[0]);
                    }
                    if (cmUsers.Count > 1)
                    {
                        CMSecondShow.Visibility = Visibility.Visible;
                        setOneContactMan(CMFirstName2, CMSecondName2, CMThirdName2,
                            CMPhone21, CMPhone22, CMPhone23, cmUsers[1]);
                    }
                    if (cmUsers.Count > 2)
                    {
                        CMThirdShow.Visibility = Visibility.Visible;
                        setOneContactMan(CMFirstName3, CMSecondName3, CMThirdName3,
                            CMPhone31, CMPhone32, CMPhone33, cmUsers[2]);
                    }
                };

            setAllContactMans(CurrentUser.P_ManForContact.ToList());
            M_Departments DepCurrentUser = CurrentUser.P_Departments;
            TB_DepName.Text = DepCurrentUser.Name;
            TB_DepCode.Text = DepCurrentUser.CodeFull;
            M_Names derName = DepCurrentUser.DepartmentDirectorName.Single();
            TB_DepFN.Text = derName.NameFirst;
            TB_DepSN.Text = derName.NameLast;
            TB_DepTN.Text = derName.NameThird;
            setPhones(DepartmentPhoneNumber1, DepartmentPhoneNumber2, DepartmentPhoneNumber3, DepCurrentUser.DepartmentDirectorPhone.ToList());
            //Rfid
            ShowRfid.Text = CurrentUser.P_Rfids.Last().RfidID.ToString();
            //  WorkTime
            var timeCurrentUser = CurrentUser.P_InOutValidTimes;
            TB_Start.SelectedTime = timeCurrentUser.Start.TimeOfDay;
            TB_End.SelectedTime = timeCurrentUser.End.TimeOfDay;
            TB_Valid.SelectedTime = timeCurrentUser.Valid.TimeOfDay;

            if (timeCurrentUser.Dinner == null)
            {
                TB_Dinner.SelectedTime = DateTime.Now.TimeOfDay;
            }

            TB_Dinner.SelectedTime = ((DateTime)timeCurrentUser.Dinner).TimeOfDay; 
            CB_IsAdmin.IsChecked = CurrentUser.IsAdmin;
            CB_IsDirecor.IsChecked = CurrentUser.IsDirector;
        }
        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(UploadImageSelection, e);
            ToolTipService.SetIsEnabled(UserInformationSelection, e);
            ToolTipService.SetIsEnabled(ContactManSelection, e);
            ToolTipService.SetIsEnabled(TimeWork, e);
            ToolTipService.SetIsEnabled(DepartmentInfoSelection, e);
            ToolTipService.SetIsEnabled(RfiNumbertSelection, e);
            ToolTipService.SetIsEnabled(upload, e);
            ToolTipService.SetIsEnabled(SaveChanges, e);
            ToolTipService.SetIsEnabled(Button_AddUserPhone, e);
            ToolTipService.SetIsEnabled(Button_AddUserPhone, e);
            ToolTipService.SetIsEnabled(Button_AddContacnManPhone, e); 
            ToolTipService.SetIsEnabled(Button_AddContacnMan, e);
            ToolTipService.SetIsEnabled(Button_AddDirectorPhone, e);          
            ToolTipService.SetIsEnabled(AddRfid, e);
        }
        public void HideGrids()
        {
            UploadImageGrid.Visibility = Visibility.Collapsed;
            UserInformationGrid.Visibility = Visibility.Collapsed;
            ContactManGrid.Visibility = Visibility.Collapsed;
            WorkTimeGrid.Visibility = Visibility.Collapsed;
            DepatmantInformationGrid.Visibility = Visibility.Collapsed;
            RfidNumberGrid.Visibility = Visibility.Collapsed;
        }
        public void CreateContactMan(List<M_Users> contactManList)
        {
            string firstName;
            string secondName;
            string thirdName;

            if (TB_FN.Text != string.Empty &&
                TB_SN.Text != string.Empty &&
                    TB_TN.Text != string.Empty)
            {
                firstName = TB_FN.Text;
                secondName = TB_SN.Text;
                thirdName = TB_TN.Text;
            }
            else
            {
                MessageBox.Show((string)Application.Current.Resources["msb_setPIB"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Action<Grid, TextBox, TextBox, TextBox> showAndSet = (grid, tb1, tb2, tb3) =>
            {
                grid.Visibility = Visibility.Visible;
                tb1.Text = firstName;
                tb2.Text = secondName;
                tb3.Text = thirdName;
            };

            Action<TextBox, TextBox, TextBox> setNumbers = (tb1, tb2, tb3) =>
            {
                if (TB_ContacnManPhone.Text == string.Empty)
                {
                    if (ContactPhonNumber1.Text != string.Empty)
                    {
                        tb1.Text = ContactPhonNumber1.Text;
                    }
                    if (ContactPhonNumber2.Text != string.Empty)
                    {
                        tb2.Text = ContactPhonNumber2.Text;
                    }
                    if (ContactPhonNumber3.Text != string.Empty)
                        tb3.Text = ContactPhonNumber3.Text;
                }
                else
                {
                    tb1.Text = TB_ContacnManPhone.Text;
                }

            };

            if (CMFirstShow.Visibility == Visibility.Collapsed)
            {
                showAndSet(CMFirstShow, CMFirstName1, CMSecondName1, CMThirdName1);
                setNumbers(CMPhone11, CMPhone12, CMPhone13);
            }
            else if (CMSecondShow.Visibility == Visibility.Collapsed)
            {
                showAndSet(CMSecondShow, CMFirstName2, CMSecondName2, CMThirdName2);
                setNumbers(CMPhone21, CMPhone22, CMPhone23);
            }
            else if (CMThirdShow.Visibility == Visibility.Collapsed)
            {
                showAndSet(CMThirdShow, CMFirstName3, CMSecondName3, CMThirdName3);
                setNumbers(CMPhone31, CMPhone32, CMPhone33);
            }

        }
        public void AddPhoneNumber(TextBox out1, TextBox out2, TextBox out3, TextBox source)
        {
            Func<TextBox, bool> SetTextBoxNumber = (outp) =>
            {
                bool b;
                if (b = (outp.Text == string.Empty))
                {
                    outp.Text = source.Text;
                }
                return b;
            };


            if (SetTextBoxNumber(out1)) { }
            else if (SetTextBoxNumber(out2)) { }
            else if (SetTextBoxNumber(out3)) { }

            source.Text = string.Empty;


        }
        public bool ChangeUserFalg { get; set; } = false;
        public bool IsImageExist { get; private set; }
        public M_Users CurrentUser { get; private set; }
        public DirectorSearcherHelper AutoCompliteDirectorHelper { get; private set; }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            //basic fields cheking
            if (string.IsNullOrEmpty(TB_UserFN.Text) || string.IsNullOrEmpty(TB_UserSN.Text) || string.IsNullOrEmpty(TB_UserTN.Text))
            {
                MessageBox.Show((string)Application.Current.Resources["S_Ex_emptyNameSurname"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(TB_DepName.Text) || string.IsNullOrEmpty(TB_DepCode.Text))
            {
                MessageBox.Show((string)Application.Current.Resources["S_Ex_emptyDepInfo"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //add or update user info
            if (iD != null)
            {
                var user = db.C_Users.Find(iD);
                AddUserHelper.UpdateUserInfo(UserInfoGrid, user);
                AddUserHelper.UpdateContactsInfo(ContactManInfoGrid, user);
                AddUserHelper.UpdateInOutTimes(WorkTimeInfoGrid, user);
                user.Photo = AddUserHelper.GetPhotoPath(user, IsImageExist, imagePath);

                //rfid checks and update 
                var newRfid = Convert.ToInt64(ShowRfid.Text);
                if (!(user.P_Rfids.FirstOrDefault().RfidID == newRfid))
                {
                    foreach (var item in db.C_Rfids)
                    {
                        if (item.RfidID == newRfid && item.IsArhive)
                        {
                            item.IsArhive = false;
                            user.P_Rfids = new List<M_Rfids> {item};
                        }
                        else if (item.RfidID == newRfid && !item.IsArhive)
                        {
                            MessageBox.Show((string)Application.Current.Resources["S_Ex_RfidBindErr"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            user.P_Rfids = new List<M_Rfids> { AddUserHelper.GetRfidInfo(RfidGrid) };
                        }
                    }
                }

                //departments checks and update
                var newDepName = TB_DepName.Text;
                var newDepCode = TB_DepCode.Text;
                var isNoFind = true;
                foreach (var dep in db.C_Departments)
                {
                    if (dep.Name == newDepName && dep.CodeFull == newDepCode)
                    {
                        user.P_Departments.CodeFull = dep.CodeFull;
                        user.P_Departments.Name = dep.Name;

                        user.P_Departments.DepartmentDirectorName.FirstOrDefault().NameFirst =
                            string.IsNullOrEmpty(dep.DepartmentDirectorName.FirstOrDefault().NameFirst)
                                ? null
                                : dep.DepartmentDirectorName.FirstOrDefault().NameFirst;
                        user.P_Departments.DepartmentDirectorName.FirstOrDefault().NameLast =
                            string.IsNullOrEmpty(dep.DepartmentDirectorName.FirstOrDefault().NameLast)
                                ? null
                                : dep.DepartmentDirectorName.FirstOrDefault().NameLast;
                        user.P_Departments.DepartmentDirectorName.FirstOrDefault().NameThird =
                            string.IsNullOrEmpty(dep.DepartmentDirectorName.FirstOrDefault().NameThird)
                                ? null
                                : dep.DepartmentDirectorName.FirstOrDefault().NameThird;
                        user.P_Departments.DepartmentDirectorPhone.FirstOrDefault().PhoneNumber =
                            string.IsNullOrEmpty(dep.DepartmentDirectorPhone.FirstOrDefault().PhoneNumber)
                                ? null
                                : dep.DepartmentDirectorPhone.FirstOrDefault().PhoneNumber;
                        isNoFind = false;
                        break;
                    }
                }
                if (isNoFind)
                {
                    user.P_Departments = AddUserHelper.GetDepartmentInfo(DepartmentInfoGrid, user);
                }

                db.SaveChanges();
            }
            else
            {
                //create new user with input info
                var newUser = AddUserHelper.GetNewUserInfo(UserInfoGrid);
                newUser.P_ManForContact = AddUserHelper.GetAllContactsInfo(ContactManInfoGrid, newUser);
                newUser.P_InOutValidTimes = AddUserHelper.GetInOutValidTimes(WorkTimeInfoGrid);
                newUser.Photo = AddUserHelper.GetPhotoPath(newUser, IsImageExist, imagePath);

                //department adding & checking for existing departments
                var isNoFind = true;
                foreach (var dep in db.C_Departments)
                {
                    if (dep.Name == TB_DepName.Text && dep.CodeFull == TB_DepCode.Text)
                    {
                        dep.P_Users.Add(newUser);
                        isNoFind = false;
                        break;
                    }
                }
                if (isNoFind)
                {
                    newUser.P_Departments = AddUserHelper.GetDepartmentInfo(DepartmentInfoGrid, newUser);
                }

                //rfid checks and adding 
                var changeRfid = true;
                foreach (var itemRfid in db.C_Rfids)
                {
                    if (itemRfid.RfidID == Convert.ToInt64(ShowRfid.Text) && itemRfid.IsArhive)
                    {
                        itemRfid.IsArhive = false;
                        itemRfid.P_Users = newUser;
                        changeRfid = false;
                        break;
                    }
                    if (itemRfid.RfidID == Convert.ToInt64(ShowRfid.Text) && !itemRfid.IsArhive)
                    {
                        MessageBox.Show((string)Application.Current.Resources["S_Ex_RfidBindErr"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if (changeRfid)
                {
                    newUser.P_Rfids = new List<M_Rfids> { AddUserHelper.GetRfidInfo(RfidGrid) };
                }
                

                db.C_Users.Add(newUser);
                db.SaveChanges();
            }

            var p = new StartUpPage();
            Singelton.Frame.NavigationService.Navigate(p);
            
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
                imgPhoto.Source = ImageLoaderHelper.GetImageFromFolder(op.FileName);
                imagePath = op.FileName;
                IsImageExist = true;
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
            AddPhoneNumber(PhoneNumber1, PhoneNumber2, PhoneNumber3, TB_UserPhone);
        }
        private void Button_AddContactManPhone_Click(object sender, RoutedEventArgs e)
        {
            AddPhoneNumber(ContactPhonNumber1, ContactPhonNumber2, ContactPhonNumber3, TB_ContacnManPhone);
        }
        private void Button_AddContactMan_Click(object sender, RoutedEventArgs e)
        {
            CreateContactMan(contactMan);
            TB_FN.Text = string.Empty;
            TB_SN.Text = string.Empty;
            TB_TN.Text = string.Empty;
            ContactPhonNumber1.Text = string.Empty;
            ContactPhonNumber2.Text = string.Empty;
            ContactPhonNumber3.Text = string.Empty;
        }
        private void Button_AddDirectPhone_Click(object sender, RoutedEventArgs e)
        {
            AddPhoneNumber(DepartmentPhoneNumber1, DepartmentPhoneNumber2, DepartmentPhoneNumber3, TB_DirectorPhone);
        }
        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            StartUpPage p = new StartUpPage();
            this.NavigationService.Navigate(p);
        }
        private void Click_AddRfid(object sender, RoutedEventArgs e)
        {
            PortDataReceivedHelper portDataReceived = new PortDataReceivedHelper();
            ShowRfid.Text = portDataReceived.ParseInData.ToString();
        }
        private void UploadImageSelection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_UpdateImage"]);
            HideGrids();
            UploadImageGrid.Visibility = Visibility.Visible;
        }
        private void UserInformationSelection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_UserInformation"]);
            HideGrids();
            UserInformationGrid.Visibility = Visibility.Visible;
        }
        private void ContactManSelection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_ContactPeople"]);
            HideGrids();
            ContactManGrid.Visibility = Visibility.Visible;
        }
        private void TimeWork_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_WorkTime"]);
            HideGrids();
            WorkTimeGrid.Visibility = Visibility.Visible;
        }
        private void DepartmentInfoSelection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_lbi_DepInfo"]);
            HideGrids();
            DepatmantInformationGrid.Visibility = Visibility.Visible;
        }
        private void RfiNumbertSelection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Singelton.MainWindow.ChangeStringAddres(Title + " > " + Application.Current.Resources["addu_RfidNumber"]);
            HideGrids();
            RfidNumberGrid.Visibility = Visibility.Visible;
        }
        private void ContactPhonNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).IsEnabled = false;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ContactPhonNumber1.IsEnabled = true;
            FocusManager.SetFocusedElement(focusetGrid, ContactPhonNumber1);
        }
        private void EditPhoneButton1_Click(object sender, RoutedEventArgs e)
        {
            ContactPhonNumber1.IsEnabled = true;
            FocusManager.SetFocusedElement(focusetGrid, ContactPhonNumber1);
        }
        private void EditPhoneButton2_Click(object sender, RoutedEventArgs e)
        {
            ContactPhonNumber2.IsEnabled = true;
            FocusManager.SetFocusedElement(focusetGrid, ContactPhonNumber2);
        }
        private void ResetButton1_Click(object sender, RoutedEventArgs e)
        {
            ContactPhonNumber1.Text = String.Empty;
        }
        private void ResetButton2_Click(object sender, RoutedEventArgs e)
        {
            ContactPhonNumber2.Text = String.Empty;
        }
        private void PhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).IsEnabled = false;
        }
        private void UserEditPhoneButton1_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumber1.IsEnabled = true;
            FocusManager.SetFocusedElement(userFocusedGrid, PhoneNumber1);
        }
        private void UserResetButton1_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumber1.Text = String.Empty;
        }
        private void UserEditPhoneButton2_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumber2.IsEnabled = true;
            FocusManager.SetFocusedElement(userFocusedGrid, PhoneNumber2);
        }
        private void UserResetButton2_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumber2.Text = String.Empty;
        }
        private void DepartamentEditButton1_Click(object sender, RoutedEventArgs e)
        {
            DepartmentPhoneNumber1.IsEnabled = true;
            FocusManager.SetFocusedElement(departamentFocusetGrid, DepartmentPhoneNumber1);
        }
        private void DepartamentRemoveButton1_Click(object sender, RoutedEventArgs e)
        {
            DepartmentPhoneNumber1.Text = String.Empty;
        }
        private void DepartamentEditButton2_Click(object sender, RoutedEventArgs e)
        {
            DepartmentPhoneNumber2.IsEnabled = true;
            FocusManager.SetFocusedElement(departamentFocusetGrid, DepartmentPhoneNumber2);
        }
        private void DepartamentRemoveButton2_Click(object sender, RoutedEventArgs e)
        {
            DepartmentPhoneNumber2.Text = String.Empty;
        }
        private void DepartmentPhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).IsEnabled = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AutoCompliteDirectorHelper = new DirectorSearcherHelper();
            AutoCompliteDirectorHelper.FactoryDirectors();
            AutoCompliteDirectorHelper.SetAutoComplite(TB_Search);

            SetEnableToolTip();
          
            if (iD == null)
            {
                Singelton.MainWindow.ChangeStringAddres(Title);
                //Clear and Hide all fields
                CMFirstShow.Visibility = Visibility.Collapsed;
                CMSecondShow.Visibility = Visibility.Collapsed;
                CMThirdShow.Visibility = Visibility.Collapsed;
                CMFirstName1.Text = String.Empty;
                CMFirstName2.Text = String.Empty;
                CMFirstName3.Text = String.Empty;
                CMSecondName1.Text = String.Empty;
                CMSecondName2.Text = String.Empty;
                CMSecondName3.Text = String.Empty;
                CMThirdName1.Text = String.Empty;
                CMThirdName2.Text = String.Empty;
                CMThirdName3.Text = String.Empty;
                CMPhone11.Text = String.Empty;
                CMPhone12.Text = String.Empty;
                CMPhone13.Text = String.Empty;
                CMPhone21.Text = String.Empty;
                CMPhone22.Text = String.Empty;
                CMPhone23.Text = String.Empty;
                CMPhone31.Text = String.Empty;
                CMPhone32.Text = String.Empty;
                CMPhone33.Text = String.Empty;
                DP_UserBithday.SelectedDate = new DateTime(1990, 1, 1, 12, 0, 0);
                TB_Start.SelectedTime = new TimeSpan(9, 0, 0);
                TB_End.SelectedTime = new TimeSpan(17, 0, 0);
                TB_Valid.SelectedTime = new TimeSpan(0, 15, 0);
                TB_Dinner.SelectedTime = new TimeSpan(13, 0, 0);
            }
        }

        private void ResetButton3_Click(object sender, RoutedEventArgs e)
        {
            ContactPhonNumber3.Text = String.Empty;
        }
        private void DepartamentRemoveButton3_Click(object sender, RoutedEventArgs e)
        {
            DepartmentPhoneNumber3.Text = string.Empty;
        }
        private void UserResetButton3_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumber3.Text = string.Empty;
        }
        private void TB_Search_ItemSelected(object sender, EventArgs e)
        {
            
            Directors directorInfo = (from dir in AutoCompliteDirectorHelper.Directors
                                      where dir.FullSearchString == TB_Search.SelectedItem
                                      select dir).FirstOrDefault();
            TB_DepName.Text = directorInfo.NameDepart;
            TB_DepCode.Text = directorInfo.CodeDepart;
            TB_DepFN.Text = directorInfo.First;
            TB_DepSN.Text = directorInfo.Last;
            TB_DepTN.Text = directorInfo.Third;
            TB_DirectorPhone.Text = directorInfo.PhoneNumber;
        }
        public void AutoTestToSaveChanges(string i="14")
        {
            
            //UserInformationSelection
            TB_UserFN.Text = "B4dasfdsfs";
            TB_UserSN.Text = "43fdsfds3C";
            TB_UserTN.Text = "User Third Name";
            TB_UsesrAddr.Text = "User Addres";
            PhoneNumber1.Text = "097111111";
            PhoneNumber2.Text = "097111112";
            PhoneNumber3.Text = "097111113";

            //cm
            CMFirstShow.Visibility = Visibility.Visible;
            CMSecondShow.Visibility = Visibility.Visible;
            CMThirdShow.Visibility = Visibility.Visible;

            CMFirstName1.Text = "Contact man1 first name" + i;
            CMSecondName1.Text = "Contact man1 second name" + i;
            CMThirdName1.Text = "Contact man1 second name" + i;
            CMPhone11.Text = "097111111" + i;
            CMPhone12.Text = "097111112" + i;
            CMPhone13.Text = "097111113" + i;
            CMFirstName2.Text = "Contact man2 first name" + i;
            CMSecondName2.Text = "Contact man2 second name" + i;
            CMThirdName2.Text = "Contact man2 second name" + i;
            CMPhone21.Text = "097111121" + i;
            CMPhone22.Text = "097111122" + i;
            CMPhone23.Text = "097111123" + i;
            CMFirstName3.Text = "Contact man3 first name" + i;
            CMSecondName3.Text = "Contact man3 second name" + i;
            CMThirdName3.Text = "Contact man3 second name" + i;
            CMPhone31.Text = "097111131" + i;
            CMPhone32.Text = "097111132" + i;
            CMPhone33.Text = "09711113" + i;
            //
            TB_DepName.Text = "DepartmentName" + i;
            TB_DepCode.Text = "11111" + i;
            TB_DepFN.Text = "Director First Name" + i;
            TB_DepSN.Text = "Director Second Name" + i;
            TB_DepTN.Text = "Director Third Name" + i;
            DepartmentPhoneNumber1.Text = "097111111" + i;
            DepartmentPhoneNumber2.Text = "097111112" + i;
            DepartmentPhoneNumber3.Text = "097111113" + i;
            //
            ShowRfid.Text = "1111111";



        }

        private void TB_End_SelectedTimeChanged(object sender, Metro.TimePickerBaseSelectionChangedEventArgs<TimeSpan?> e)
        {

            if (String.Compare(TB_End.SelectedTime.ToString(), TB_Start.SelectedTime.ToString()) != 1 && TB_End.SelectedTime != null && TB_Start.SelectedTime != null)
            {
                MessageBox.Show(Application.Current.Resources["S_Ex_InvalidTime"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TB_Start.SelectedTime = new TimeSpan(8,0,0);
                TB_End.SelectedTime = new TimeSpan(18,0,0);
            }
        }

        private void TB_UserPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.Match(TB_UserPhone.Text, @"^(\d{7}|\d{10})$").Success)
            {
                TB_UserPhone.Background = Brushes.Green;
                Button_AddUserPhone.IsEnabled = true;
            }
            else
            {
                TB_UserPhone.Background = Brushes.LightCoral;
                Button_AddUserPhone.IsEnabled = false;
            }
            
        }

        private void TB_ContacnManPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.Match(TB_ContacnManPhone.Text, @"^(\d{7}|\d{10})$").Success)
            {
                TB_ContacnManPhone.Background = Brushes.Green;
                Button_AddContacnManPhone.IsEnabled = true;
            }
            else
            {
                TB_ContacnManPhone.Background = Brushes.LightCoral;
                Button_AddContacnManPhone.IsEnabled = false;
            }
        }

        private void TB_DirectorPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.Match(TB_DirectorPhone.Text, @"^(\d{7}|\d{10})$").Success)
            {
                TB_DirectorPhone.Background = Brushes.Green;
                Button_AddDirectorPhone.IsEnabled = true;
            }
            else
            {
                TB_DirectorPhone.Background = Brushes.LightCoral;
                Button_AddDirectorPhone.IsEnabled = false;
            }
        }
    }

}

