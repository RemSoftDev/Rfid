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
using System.Windows.Shapes;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for AllDate.xaml
    /// </summary>
    public partial class AllDate : Window
    {
        int Id;
        public AllDate(int a)
        {
            InitializeComponent();

            Id = a;
            labelWeekInside.Content ="Week : " + WeekMonthYearInside(DateTime.Now.AddDays(-7), DateTime.Now);
            labelMonthInside.Content ="Month : " + WeekMonthYearInside(DateTime.Now.AddMonths(-1), DateTime.Now);
            labelYearInside.Content ="Year : " + WeekMonthYearInside(DateTime.Now.AddYears(-1), DateTime.Now);

            labelWeekOutside.Content = "Week : " + WeekMonthYearOutside(DateTime.Now.AddDays(-7), DateTime.Now);
            labelMonthOutside.Content = "Month : " + WeekMonthYearOutside(DateTime.Now.AddMonths(-1), DateTime.Now);
            labelYearOutside.Content = "Year : " + WeekMonthYearOutside(DateTime.Now.AddYears(-1), DateTime.Now);

        }

        private string WeekMonthYearInside(DateTime dateFrom, DateTime dateTo)
        {
            RfidContext db = new RfidContext();
            try
            {

                var queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.P_Users.ID == Id && cust.SingleDate > dateFrom && cust.SingleDate < dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };

                queryAllDepartaments.ToList();



                double sumDate = 0;

                foreach (var temp in queryAllDepartaments)
                {
                    sumDate += temp.LengthOfInside.Value.TimeOfDay.TotalMinutes;
                }
                return sumDate.ToString();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.ToString());
                return "Exception";
            }
        }

        private string WeekMonthYearOutside(DateTime dateFrom, DateTime dateTo)
        {
            RfidContext db = new RfidContext();
            try
            {

                var queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.P_Users.ID == Id && cust.SingleDate > dateFrom && cust.SingleDate < dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };

                queryAllDepartaments.ToList();



                double sumDate = 0;

                foreach (var temp in queryAllDepartaments)
                {
                    if (temp.LengthOfOutside != null)
                    {
                        sumDate += temp.LengthOfOutside.Value.TimeOfDay.TotalMinutes;
                    }
                }
                return sumDate.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                return "Exception";
            }
        }

        private void buttonGetData_Click(object sender, RoutedEventArgs e)
        {
            RfidContext db = new RfidContext();
            try
            {
                DateTime dateFrom = CalendarFrom.SelectedDate.Value;
                DateTime dateTo = CalendarTo.SelectedDate.Value;



                var queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.P_Users.ID == Id && cust.SingleDate > dateFrom && cust.SingleDate < dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };

                queryAllDepartaments.ToList();



                double sumDateInside = 0;
                double sumDateOutside = 0;

                foreach (var temp in queryAllDepartaments)
                {
                    if (temp.LengthOfInside != null)
                        sumDateInside += temp.LengthOfInside.Value.TimeOfDay.TotalMinutes;

                    if (temp.LengthOfOutside != null)
                        sumDateOutside += temp.LengthOfOutside.Value.TimeOfDay.TotalMinutes;
                }
                labelSelectedTimeInside.Content = "Time by selected date : " + sumDateInside.ToString();
                labelSelectedTimeOutside.Content = "Time by selected date : " + sumDateOutside.ToString();

            }
            catch
            {
                MessageBox.Show(" Please select date ");
            }
        }
    }
}
