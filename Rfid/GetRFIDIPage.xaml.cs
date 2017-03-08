using Rfid.Context;
using Rfid.Models;
using Rfid.Setting;
using Rfid.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for GetRFIDIdWindow.xaml
    /// </summary>
    public partial class GetRFIDIPage : Page
    {
        public GetRFIDIPage()
        {
            InitializeComponent();
            Watcher = Singelton.WatcherSetting;
        }
        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(BOK, e);
        }
        public LimitInsideSetting Watcher { get; set; }

        public string RfidNumber { get; private set; }
        public M_Users AuthorithationUser { get; private set; }

        private void WriteById()
        {
            try
            { 
                RfidContext db = new RfidContext();
                long s = Convert.ToInt64(RfidNumber);
                var user = db.Database.SqlQuery<M_Users>(new SqlSearchUserForRfid().SQl_SearchUserForRfid, s).SingleOrDefault();
                M_Users dsd = db.C_Users.Where(z => z.ID == user.ID).Single();
                AuthorithationUser = dsd;

                if (dsd.isInside == false)
                {
                    TimeSpan? lengthOfInside = new TimeSpan();

                    if (dsd.P_UserTime.Count != 0 )
                    {
                        lengthOfInside = DateTime.Now - dsd.P_UserTime.Last().TimeOut;
                        DateTime date = DateTime.Today.Add(lengthOfInside.Value);
                        DateTime? dtOutsige = DateTime.Today.Add(lengthOfInside.Value);
                        dsd.P_UserTime.Last().LengthOfOutside = dsd.P_UserTime.Last().Day == DateTime.Today.DayOfWeek.ToString() ? dtOutsige : null;
                    }

                    dsd.P_UserTime.Add(new M_UserTime { SingleDate = DateTime.Now.Date });
                    dsd.P_UserTime.Last().Day = DateTime.Now.DayOfWeek.ToString();
                    dsd.P_UserTime.Last().TimeIn = DateTime.Now;
                    dsd.isInside = true;
                }
                else
                {
                    dsd.P_UserTime.Last().TimeOut = DateTime.Now;
                    TimeSpan? lengthOfInside = dsd.P_UserTime.Last().TimeOut - dsd.P_UserTime.Last().TimeIn;

                    if (lengthOfInside > Singelton.WatcherSetting.MaxTimeInside)
                    {
                        lengthOfInside = Singelton.WatcherSetting.MaxTimeInside;
                    }

                    DateTime? dtInsige = DateTime.Today.Add(lengthOfInside.Value);
                    dsd.P_UserTime.Last().LengthOfInside = dtInsige;
                    dsd.isInside = false;
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RfidNumber = TBGetRfidId.Text;
                WriteById();
                AboutUserPage p = new AboutUserPage(AuthorithationUser.ID);
            
            Singelton.Frame.NavigationService.Navigate(p);
            }
            catch 
            {
                MessageBox.Show((string)Application.Current.Resources["GRW_Err_EnterRfidPleas"], "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
            Keyboard.Focus(TBGetRfidId);
        }

        private void TBGetRfidId_TextChanged(object sender, TextChangedEventArgs e)
        {
            BOK.IsEnabled = Regex.Match(TBGetRfidId.Text, @"^(\d{1,10})$").Success;
        }
    }
}

