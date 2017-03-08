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
using System.IO.Ports;
using Rfid.Helpers;
using Rfid.Context;
using Rfid.Models;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for UserInfo_Main.xaml
    /// </summary>
    public partial class UserInfo_Main : Page
    {
        public static string[] checkedPort = SerialPort.GetPortNames();

        public UserInfo_Main()
        {
            InitializeComponent();

            if (checkedPort.Length == 0)
            {
                //B_RfidStart.IsEnabled = false;

                T_ErrorConnectMessage.Visibility = new Visibility();

            }
        }

        private void Button_User_Find(object sender, RoutedEventArgs e)
        {
            UserInfo_Find p = new UserInfo_Find();
            this.NavigationService.Navigate(p);
        }

        private void Button_User_Add(object sender, RoutedEventArgs e)
        {
            UserInfo_Add p = new UserInfo_Add();
            this.NavigationService.Navigate(p);
        }

        private void Button_RfidStart(object sender, RoutedEventArgs e)
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

        private void Button_BigInfo(object sender, RoutedEventArgs e)
        {
            UsersBigInfo p = new UsersBigInfo();
            this.NavigationService.Navigate(p);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            FP p = new FP();
            this.NavigationService.Navigate(p);
        }
    }

}
