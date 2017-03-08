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
    /// <summary>
    /// Interaction logic for LimitSetting.xaml
    /// </summary>
    public partial class LimitSetting : Page
    {
        public LimitSetting()
        {
            InitializeComponent();
        }

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(SaveChanges, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Singelton.WatcherSetting.MaxTimeInside = (TimeSpan)LimitPicker.SelectedTime;
            Singelton.WatcherSetting.Interval = (TimeSpan)IntervalPicker.SelectedTime;
            using (var db = new RfidContext())
            {
                db.C_Setting.SingleOrDefault().ExportInBdSetting();
                db.SaveChanges();
            }
                if (Singelton.Frame.CanGoBack)
                {
                    Singelton.Frame.GoBack();
                }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            LimitPicker.SelectedTime = Singelton.WatcherSetting.MaxTimeInside;
            IntervalPicker.SelectedTime = Singelton.WatcherSetting.Interval;
            Singelton.MainWindow.ChangeStringAddres(Title);
        }
    }
}
