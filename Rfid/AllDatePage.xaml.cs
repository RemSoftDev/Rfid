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

namespace Rfid
{
    public partial class AllDatePage : Page
    {              
        public AllDatePage(int? a = null) 
        {
            InitializeComponent();

            Id = a;
            
            labelWeekInside.Content = Application.Current.Resources["at_Week"] + WeekMonthYearInside(DateTime.Now.AddDays(-7), DateTime.Now);
            labelMonthInside.Content = Application.Current.Resources["at_Month"] + WeekMonthYearInside(DateTime.Now.AddMonths(-1), DateTime.Now);
            labelYearInside.Content = Application.Current.Resources["at_Year"] + WeekMonthYearInside(DateTime.Now.AddYears(-1), DateTime.Now);

            labelWeekOutside.Content = Application.Current.Resources["at_Week"] + WeekMonthYearOutside(DateTime.Now.AddDays(-7), DateTime.Now);
            labelMonthOutside.Content = Application.Current.Resources["at_Month"] + WeekMonthYearOutside(DateTime.Now.AddMonths(-1), DateTime.Now);
            labelYearOutside.Content = Application.Current.Resources["at_Year"] + WeekMonthYearOutside(DateTime.Now.AddYears(-1), DateTime.Now);

        }

        int? Id;

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(buttonGetData, e);
        }

        private string WeekMonthYearInside(DateTime dateFrom, DateTime dateTo)
        {
            RfidContext db = new RfidContext();

            try
            { 
                IQueryable<dynamic> queryAllDepartaments;
                if (Id == null)
                {
                    queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.SingleDate >= dateFrom && cust.SingleDate < dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };
                }
                else
                {
                    queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.P_Users.ID == Id && cust.SingleDate >= dateFrom && cust.SingleDate < dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };
                }
                
                var sumDate = DateTime.MinValue;

                foreach (var temp in queryAllDepartaments)
                {
                    if (temp.LengthOfInside != null)
                    {
                        sumDate = sumDate.Add(((DateTime?)temp.LengthOfInside).Value.TimeOfDay);
                    }
                }
                var timeRes = new TimeSpan(sumDate.Ticks);
                return string.Format(" {0:####}h {1}m", Math.Truncate(timeRes.TotalHours), timeRes.Minutes);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }

            return string.Empty;
        }
        private string WeekMonthYearOutside(DateTime dateFrom, DateTime dateTo)
        {
            RfidContext db = new RfidContext();
            try
            {
                IQueryable<dynamic> queryAllDepartaments;
                if (Id == null)
                {
                    queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.SingleDate >= dateFrom && cust.SingleDate < dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };
                }
                else
                {
                    queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.P_Users.ID == Id && cust.SingleDate >= dateFrom && cust.SingleDate < dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };
                }
                var sumDate = DateTime.MinValue;

                foreach (var temp in queryAllDepartaments)
                {
                    if (temp.LengthOfOutside != null)
                    {
                        sumDate = sumDate.Add(((DateTime?)temp.LengthOfOutside).Value.TimeOfDay);
                    }
                }
                var timeRes = new TimeSpan(sumDate.Ticks);
                return string.Format(" {0:####}h {1}m", Math.Truncate(timeRes.TotalHours), timeRes.Minutes);
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

                IQueryable<dynamic> queryAllDepartaments;
                if (Id == null)
                {
                    queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.SingleDate >= dateFrom && cust.SingleDate <= dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };
                }
                else
                {
                    queryAllDepartaments = from cust in db.C_UserTime
                                           where cust.P_Users.ID == Id && cust.SingleDate >= dateFrom && cust.SingleDate <= dateTo
                                           select new { cust.SingleDate, cust.LengthOfInside, cust.LengthOfOutside };
                }
               
                var sumDateInside = DateTime.MinValue;
                var sumDateOutside = DateTime.MinValue;

                foreach (var temp in queryAllDepartaments)
                {
                    if (temp.LengthOfInside != null)
                        sumDateInside = sumDateInside.Add(((DateTime?)temp.LengthOfInside).Value.TimeOfDay);

                    if (temp.LengthOfOutside != null)
                        sumDateOutside = sumDateOutside.Add(((DateTime?)temp.LengthOfOutside).Value.TimeOfDay);
                }

                var timeInside = new TimeSpan(sumDateInside.Ticks);
                var timeOutside = new TimeSpan(sumDateOutside.Ticks);
                labelSelectedTimeInside.Content = Application.Current.Resources["at_TimeSelected"] + string.Format(" {0:####}h {1}m", Math.Truncate(timeInside.TotalHours), timeInside.Minutes); 
                labelSelectedTimeOutside.Content = Application.Current.Resources["at_TimeSelected"] + string.Format(" {0:####}h {1}m", Math.Truncate(timeOutside.TotalHours), timeOutside.Minutes);

        }
            catch(Exception)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_notSelectDate"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
}
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
        }

       
    }
}
