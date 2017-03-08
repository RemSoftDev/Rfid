using Rfid.Context;
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
using Rfid.Helpers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for AboutUser.xaml
    /// </summary>
    public partial class AboutUserPage : Page, IProvideAccess
    {
        public AboutUserPage(int Id)
        {
            InitializeComponent();
            ID = Id;
            Run(Id);
        }

        private int ID;
        private bool _isInside;
        private RfidContext _db;

        public bool IsInside
        {
            get
            {
                return _isInside;
            }
            set
            {
                if (value)
                {
                    StatusGrid.Background = new SolidColorBrush(Colors.Green);
                    StatusValue.Text = Singelton.LanguageSetting.GetString("au_l_StatusOn");
                    ((TextBlock)((StackPanel)ChangeStatus.Content).Children[0]).Text = Application.Current.Resources["au_btnComout"].ToString();
                }
                else
                {
                    StatusGrid.Background = new SolidColorBrush(Colors.Red);
                    StatusValue.Text = Singelton.LanguageSetting.GetString("au_l_StatusOff");
                    ((TextBlock)((StackPanel)ChangeStatus.Content).Children[0]).Text = Application.Current.Resources["au_btnComin"].ToString();
                }

                _isInside = value;
            }
        }
        public M_Users CurrentUser { get; private set; }
        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(StatusGrid, e);
            ToolTipService.SetIsEnabled(button, e);
            ToolTipService.SetIsEnabled(EditUserButton, e);
            ToolTipService.SetIsEnabled(GoToReportButton, e);
            ToolTipService.SetIsEnabled(AddCommentExpander, e);
            ToolTipService.SetIsEnabled(ChangeStatus, e);
        }
        public void ProvideAccess()
        {
            EditUserButton.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            button.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            GoToReportButton.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
            ChangeStatus.Visibility = Singelton.IsAthorizationAdmin ? Visibility.Visible : Visibility.Collapsed;
        }
        public void Run(int Id)
        {
            _db = new RfidContext();
            CurrentUser = new M_Users();
            CurrentUser = _db.C_Users.Where(z => z.ID == Id).Single();
            M_Names names = CurrentUser.P_Names.SingleOrDefault();
            var dir = new DirectoryInfo(Singelton.PathToPhoto);

            try
            {
                string str = dir.FullName + "/" + CurrentUser.Photo.ToString();
                imageUser.ImageSource = ImageLoaderHelper.GetImageFromFolder(str);
            }
            catch
            {
            }

            LNameR.Content = names.NameFirst;
            LMiddleNameR.Content = names.NameLast;
            LSurnameR.Content = names.NameThird;
            NameUser.Text = $"{names.NameFirst} {names.NameLast}";
            IsInside = CurrentUser.isInside;

            try
            {
                LDateOfBirthR.Content = CurrentUser.D_Birth.Value.ToShortDateString();
            }
            catch (Exception ex)
            {

            }

            LAddressR.Content = CurrentUser.Address;
            List<M_Phones> userPhones = new List<M_Phones>();
            userPhones = CurrentUser.P_Phones.ToList();

            foreach (M_Phones phone in userPhones)
            {
                LPhoneR.Content += phone.PhoneNumber;

                if (phone != userPhones[userPhones.Count - 1])
                {
                    LPhoneR.Content += ", ";
                }
            }

            M_Names departmentsName = new M_Names();
            departmentsName = CurrentUser.P_Departments.DepartmentDirectorName.SingleOrDefault();

            LDepartmentR.Content = CurrentUser.P_Departments.Name;
            if (departmentsName != null)
            {
                LHeadOfDepartmentR.Content += departmentsName.NameFirst + " ";
                LHeadOfDepartmentR.Content += departmentsName.NameLast + " ";
                LHeadOfDepartmentR.Content += departmentsName.NameThird + " ";
            }

            List<M_Phones> departmentsPhones = new List<M_Phones>();
            departmentsPhones = CurrentUser.P_Departments.DepartmentDirectorPhone.ToList();


            foreach (M_Phones phone in departmentsPhones)
            {
                LPhoneNumberR.Content += phone.PhoneNumber;

                if (phone != departmentsPhones[departmentsPhones.Count - 1])
                {
                    LPhoneR.Content += ", ";
                }
            }

            try
            {
                List<M_Users> contactUsers = new List<M_Users>();
                contactUsers = CurrentUser.P_ManForContact.ToList();

                M_Names contactNames = new M_Names();
                List<M_Phones> contactPhones = new List<M_Phones>();

                contactNames = contactUsers[0].P_Names.ToList()[0];
                contactPhones = contactUsers[0].P_Phones.ToList();

                LContactDetailsFace1R.Content = contactNames.NameFirst + " " +
                                                contactNames.NameLast + " " +
                                                contactNames.NameThird + " ";

                foreach (M_Phones cPhones in contactPhones)
                {
                    LPhoneNumber1R.Content += cPhones.PhoneNumber;

                    if (cPhones != contactPhones[contactPhones.Count - 1])
                    {
                        LPhoneR.Content += ", ";
                    }
                }

                contactNames = contactUsers[1].P_Names.ToList()[0];
                contactPhones = contactUsers[1].P_Phones.ToList();

                LContactDetailsFace2R.Content = contactNames.NameFirst + " " +
                                                contactNames.NameLast + " " +
                                                contactNames.NameThird + " ";

                foreach (M_Phones cPhones in contactPhones)
                {
                    LPhoneNumber2R.Content += cPhones.PhoneNumber + " ";
                }
            }
            catch
            {

            }

            List<M_Rfids> prfids = new List<M_Rfids>();
            prfids = CurrentUser.P_Rfids.ToList();
            M_Rfids rfideCode;
            try
            {
                rfideCode = prfids.Where(x => x.IsArhive == false).Single();
                LRfidCode.Content = rfideCode.RfidID.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_rfidNotFound"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



            if (CurrentUser.Comment != null)
            {
                TBComment.Text = CurrentUser.Comment.ToString();
            }

            if (prfids[0].Comment != null)
            {
                TBRfidCOmment.Text = prfids[0].Comment.ToString();
            }
            
        }

        private void ClickLinkingButton(object sender, RoutedEventArgs e)
        {
            LinkingRfidPage p = new LinkingRfidPage(ID);
            this.NavigationService.Navigate(p);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
            ProvideAccess();
        }
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrUpdateUserPage p = new AddOrUpdateUserPage(ID);
            this.NavigationService.Navigate(p);
        }
        private void GoToReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportPage p = new ReportPage(ID);
            Singelton.Frame.Navigate(p);
        }
        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(((TextBlock)((StackPanel)ChangeStatus.Content).Children[0]).Text+ " ?","", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var dsd = CurrentUser.P_Rfids.SingleOrDefault();

                if (dsd.P_Users.isInside == false)
                {
                    TimeSpan? lengthOfInside = new TimeSpan();

                    if (dsd.P_Users.P_UserTime.Count != 0)
                    {
                        lengthOfInside = DateTime.Now - dsd.P_Users.P_UserTime.Last().TimeOut;
                        DateTime date = DateTime.Today.Add(lengthOfInside.Value);
                        DateTime? dtOutsige = DateTime.Today.Add(lengthOfInside.Value);
                        dsd.P_Users.P_UserTime.Last().LengthOfOutside = dsd.P_Users.P_UserTime.Last().Day == DateTime.Today.DayOfWeek.ToString() ? dtOutsige : null;
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

                    if (lengthOfInside > Singelton.WatcherSetting.MaxTimeInside)
                    {
                        lengthOfInside = Singelton.WatcherSetting.MaxTimeInside;
                    }
                    DateTime? dtInsige = DateTime.Today.Add(lengthOfInside.Value);
                    dsd.P_Users.P_UserTime.Last().LengthOfInside = dtInsige;
                    dsd.P_Users.isInside = false;
                }

                _db.SaveChanges();
                IsInside = CurrentUser.isInside;
            }
        }
    }
}
