using Rfid.Context;
using Rfid.Models;
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
    /// <summary>
    /// Interaction logic for CalendarAndWeekendPage.xaml
    /// </summary>
    public partial class CalendarAndWeekendPage : Page
    {
        private M_Users _selectedUser;
        private int? _selectedId;
        private CalendarType _calendarTypeSetting;

        public void SetInfoAboutSelectedUser(int? selectedId = null, M_Users selectedUser = null)
        {
            _selectedId = selectedId;
            _selectedUser = selectedUser;
            DP_SigleDate.Culture = App.Language;
            DP_StarDate.Culture = App.Language;
            DP_EndDate.Culture = App.Language;
        }    

        public CalendarAndWeekendPage()
        {
            InitializeComponent();
        }
        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(Btn_TimeOff, e);
            ToolTipService.SetIsEnabled(Btn_Truancy, e);
            ToolTipService.SetIsEnabled(Btn_Holidays, e);
            ToolTipService.SetIsEnabled(Btn_Vacation, e);
            ToolTipService.SetIsEnabled(Btn_AddInterval, e);
            ToolTipService.SetIsEnabled(Btn_AddSingle, e);
        }
        public void HideLines()
        {
            F_TimeOff.Visibility = Visibility.Hidden;
            F_Truancy.Visibility = Visibility.Hidden;
            F_Holidays.Visibility = Visibility.Hidden;
            F_Vacation.Visibility = Visibility.Hidden;
            C_ShowDate.SelectedDates.Clear();
        }
        public void ShowDate()
        {
            using (var db = new RfidContext())
            {
                List<M_Calendar> RangeDate = null;
                if (_calendarTypeSetting == CalendarType.Holideys)
                {
                    RangeDate = (from d in db.C_Calendar
                                 where (d.TypeDate == _calendarTypeSetting)
                                 select d).ToList();
                }
                else if (_selectedUser != null)
                {
                    RangeDate = (from d in db.C_Calendar
                                 where (d.P_Users.ID == _selectedId && d.TypeDate == _calendarTypeSetting)
                                 select d).ToList();
                }

                else
                {
                    MessageBox.Show((string)Application.Current.Resources["msb_selectUserOrHolidey"], "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                GetAll(RangeDate, C_ShowDate);
            }




        }
        public void GetAll(List<M_Calendar> list, Calendar cl)
        {
            foreach (M_Calendar calItem in list)
            {
                foreach (DateTime dtItem in GetRangeDates(calItem.Start, calItem.End))
                {
                    cl.SelectedDates.Add(dtItem);
                }
            }
        }
        public List<DateTime> GetRangeDates(DateTime start, DateTime end)
        {
            return Enumerable.Range(0, end.Subtract(start).Days + 1)
                     .Select(d => start.AddDays(d)).ToList();
        }

        private void Btn_TimeOff_Click(object sender, RoutedEventArgs e)
        {
            HideLines();
            _calendarTypeSetting = CalendarType.TimeOff;
            F_TimeOff.Visibility = Visibility.Visible;
            ShowDate();
        }
        private void Btn_Truancy_Click(object sender, RoutedEventArgs e)
        {
            HideLines();
            _calendarTypeSetting = CalendarType.Truancy;
            F_Truancy.Visibility = Visibility.Visible;
            ShowDate();

        }
        private void Btn_Holidays_Click(object sender, RoutedEventArgs e)
        {
            HideLines();
            _calendarTypeSetting = CalendarType.Holideys;
            F_Holidays.Visibility = Visibility.Visible;
            ShowDate();
        }
        private void Btn_Vacation_Click(object sender, RoutedEventArgs e)
        {
            HideLines();
            _calendarTypeSetting = CalendarType.Vacation;
            F_Vacation.Visibility = Visibility.Visible;
            ShowDate();
        } 
        private void Btn_AddInterval_Click(object sender, RoutedEventArgs e)
        {
            if (DP_StarDate.SelectedDate > DP_EndDate.SelectedDate)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_startIsBeggerEnd"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var date = new M_Calendar();
            try
            {
                date.Start = (DateTime)DP_StarDate.SelectedDate;
                date.End = (DateTime)DP_EndDate.SelectedDate;
                date.TypeDate = _calendarTypeSetting;
            }
            catch (FormatException)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_slectDate"], "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            using (var db = new RfidContext())
            {
                if (_selectedId != null)
                {
                    var user = from d in db.C_Users
                               where d.ID == _selectedId
                               select d;

                    user.Single().P_Calendar.Add(date); ;
                    db.SaveChanges();
                }

                else if (_calendarTypeSetting == CalendarType.Holideys)
                {
                    db.C_Calendar.Add(date);
                    db.SaveChanges();
                    return;
                }
                else
                {
                    MessageBox.Show((string)Application.Current.Resources["msb_startIsBeggerEnd"]);
                    return;
                }
                ShowDate();
            }
        }
        private void Btn_AddSingle_Click(object sender, RoutedEventArgs e)
        {
            var date = new M_Calendar();
            try
            {
                date.Start = (DateTime)DP_SigleDate.SelectedDate;
                date.End = (DateTime)DP_SigleDate.SelectedDate;
                date.TypeDate = _calendarTypeSetting;
            }
            catch (FormatException)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_slectDate"], "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            using (var db = new RfidContext())
            {
                if (_selectedId != null)
                {
                    var user = from d in db.C_Users
                               where d.ID == _selectedId
                               select d;

                    user.Single().P_Calendar.Add(date);
                    db.SaveChanges();
                }

                else if (_calendarTypeSetting == CalendarType.Holideys)
                {
                    db.C_Calendar.Add(date);
                    db.SaveChanges();
                    return;
                }
                else
                {
                    MessageBox.Show((string)Application.Current.Resources["msb_selectUserOrHolidey"], "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                ShowDate();
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            DP_StarDate.SelectedDate = DateTime.Now;
            DP_EndDate.SelectedDate = DateTime.Now;
            DP_SigleDate.SelectedDate = DateTime.Now;
            ShowDate();
        }
    }
}
